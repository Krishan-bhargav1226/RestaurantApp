using Application.Dtos.Tables;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Tables;

namespace Application.Applications.Tables
{
    public class TableApplication : ITableApplication
    {
        private readonly ITableRepository _tableRepository;
        private readonly IMapper _mapper;

        public TableApplication(
            ITableRepository tableRepository,
            IMapper mapper)
        {
            _tableRepository = tableRepository;
            _mapper = mapper;
        }

        public async Task<TableResponseDto> CreateAsync(CreateUpdateTableDto input)
        {
            var tableNumber = input.TableNumber.Trim();

            if (await _tableRepository.ExistsForBranchAsync(input.BranchId, tableNumber))
            {
                throw new InvalidOperationException("A table with this table number already exists in the branch.");
            }

            var table = _mapper.Map<Table>(input);
            table.TableNumber = tableNumber;

            var createdTable = await _tableRepository.CreateAsync(table);

            return _mapper.Map<TableResponseDto>(createdTable);
        }

        public async Task<List<TableResponseDto>> GetAllAsync()
        {
            var tables = await _tableRepository.GetAllAsync();

            return _mapper.Map<List<TableResponseDto>>(tables);
        }

        public async Task<TableResponseDto> GetByIdAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
            {
                throw new KeyNotFoundException("Table not found.");
            }

            return _mapper.Map<TableResponseDto>(table);
        }

        public async Task<TableResponseDto> UpdateAsync(
            int id,
            CreateUpdateTableDto input)
        {
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
            {
                throw new KeyNotFoundException("Table not found.");
            }

            var tableNumber = input.TableNumber.Trim();

            if (await _tableRepository.ExistsForBranchAsync(input.BranchId, tableNumber, id))
            {
                throw new InvalidOperationException("A table with this table number already exists in the branch.");
            }

            _mapper.Map(input, table);
            table.TableNumber = tableNumber;
            table.UpdatedDate = DateTime.UtcNow;

            var updatedTable = await _tableRepository.UpdateAsync(table);

            return _mapper.Map<TableResponseDto>(updatedTable);
        }

        public async Task DeleteAsync(int id)
        {
            var table = await _tableRepository.GetByIdAsync(id);

            if (table == null)
            {
                throw new KeyNotFoundException("Table not found.");
            }

            await _tableRepository.DeleteAsync(table);
        }
    }
}