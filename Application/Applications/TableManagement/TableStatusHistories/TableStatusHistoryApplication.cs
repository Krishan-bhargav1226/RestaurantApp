using Application.Dtos.TableStatusHistories;
using AutoMapper;
using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories.TableStatusHistories;
using Microsoft.EntityFrameworkCore;

namespace Application.Applications.TableStatusHistories
{
    public class TableStatusHistoryApplication : ITableStatusHistoryApplication
    {
        private readonly ITableStatusHistoryRepository _tableStatusHistoryRepository;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TableStatusHistoryApplication(ITableStatusHistoryRepository tableStatusHistoryRepository, DataContext context, IMapper mapper)
        {
            _tableStatusHistoryRepository = tableStatusHistoryRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<TableStatusHistoryResponseDto> CreateAsync(CreateUpdateTableStatusHistoryDto input)
        {
            await ValidateInputAsync(input);
            var history = _mapper.Map<TableStatusHistory>(input);
            history.StartTime ??= DateTime.UtcNow;
            var createdHistory = await _tableStatusHistoryRepository.CreateAsync(history);
            return _mapper.Map<TableStatusHistoryResponseDto>(createdHistory);
        }

        public async Task<List<TableStatusHistoryResponseDto>> GetAllAsync()
        {
            return _mapper.Map<List<TableStatusHistoryResponseDto>>(await _tableStatusHistoryRepository.GetAllAsync());
        }

        public async Task<TableStatusHistoryResponseDto> GetByIdAsync(int id)
        {
            var history = await _tableStatusHistoryRepository.GetByIdAsync(id);
            if (history == null)
                throw new KeyNotFoundException("Table status history not found.");
            return _mapper.Map<TableStatusHistoryResponseDto>(history);
        }

        public async Task<TableStatusHistoryResponseDto> UpdateAsync(int id, CreateUpdateTableStatusHistoryDto input)
        {
            var history = await _tableStatusHistoryRepository.GetByIdAsync(id);
            if (history == null)
                throw new KeyNotFoundException("Table status history not found.");

            await ValidateInputAsync(input);
            _mapper.Map(input, history);
            history.UpdatedDate = DateTime.UtcNow;
            var updatedHistory = await _tableStatusHistoryRepository.UpdateAsync(history);
            return _mapper.Map<TableStatusHistoryResponseDto>(updatedHistory);
        }

        public async Task DeleteAsync(int id)
        {
            var history = await _tableStatusHistoryRepository.GetByIdAsync(id);
            if (history == null)
                throw new KeyNotFoundException("Table status history not found.");
            await _tableStatusHistoryRepository.DeleteAsync(history);
        }

        private async Task ValidateInputAsync(CreateUpdateTableStatusHistoryDto input)
        {
            if (!Enum.IsDefined(input.Status))
                throw new InvalidOperationException("Invalid table status.");

            var table = await _context.Tables.FirstOrDefaultAsync(x => x.Id == input.TableId);
            if (table == null)
                throw new KeyNotFoundException("Table not found.");

            if (input.EndTime.HasValue && input.StartTime.HasValue && input.EndTime < input.StartTime)
                throw new InvalidOperationException("EndTime cannot be earlier than StartTime.");
        }
    }
}