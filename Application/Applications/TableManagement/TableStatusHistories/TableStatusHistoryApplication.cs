using Application.Dtos.TableStatusHistories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.TableStatusHistories;

namespace Application.Applications.TableStatusHistories
{
    public class TableStatusHistoryApplication : ITableStatusHistoryApplication
    {
        private readonly ITableStatusHistoryRepository _tableStatusHistoryRepository;
        private readonly IMapper _mapper;

        public TableStatusHistoryApplication(
            ITableStatusHistoryRepository tableStatusHistoryRepository,
            IMapper mapper)
        {
            _tableStatusHistoryRepository = tableStatusHistoryRepository;
            _mapper = mapper;
        }

        public async Task<TableStatusHistoryResponseDto> CreateAsync(CreateUpdateTableStatusHistoryDto input)
        {
            var history = _mapper.Map<TableStatusHistory>(input);

            if (history.StartTime == default)
            {
                history.StartTime = DateTime.UtcNow;
            }

            var createdHistory = await _tableStatusHistoryRepository.CreateAsync(history);

            return _mapper.Map<TableStatusHistoryResponseDto>(createdHistory);
        }

        public async Task<List<TableStatusHistoryResponseDto>> GetAllAsync()
        {
            var histories = await _tableStatusHistoryRepository.GetAllAsync();

            return _mapper.Map<List<TableStatusHistoryResponseDto>>(histories);
        }

        public async Task<TableStatusHistoryResponseDto> GetByIdAsync(int id)
        {
            var history = await _tableStatusHistoryRepository.GetByIdAsync(id);

            if (history == null)
            {
                throw new KeyNotFoundException("Table status history not found.");
            }

            return _mapper.Map<TableStatusHistoryResponseDto>(history);
        }

        public async Task<TableStatusHistoryResponseDto> UpdateAsync(
            int id,
            CreateUpdateTableStatusHistoryDto input)
        {
            var history = await _tableStatusHistoryRepository.GetByIdAsync(id);

            if (history == null)
            {
                throw new KeyNotFoundException("Table status history not found.");
            }

            _mapper.Map(input, history);
            history.UpdatedDate = DateTime.UtcNow;

            var updatedHistory = await _tableStatusHistoryRepository.UpdateAsync(history);

            return _mapper.Map<TableStatusHistoryResponseDto>(updatedHistory);
        }

        public async Task DeleteAsync(int id)
        {
            var history = await _tableStatusHistoryRepository.GetByIdAsync(id);

            if (history == null)
            {
                throw new KeyNotFoundException("Table status history not found.");
            }

            await _tableStatusHistoryRepository.DeleteAsync(history);
        }
    }
}