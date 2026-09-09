using Application.Dtos.Tables;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Tables;

namespace Application.Applications.Tables;

public class TableApplication : ITableApplication
{
    private readonly ITableRepository _repository;
    private readonly IMapper _mapper;
    public TableApplication(ITableRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }

    public async Task<TableResponseDto> CreateAsync(CreateUpdateTableDto input)
    {
        if (await _repository.ExistsForBranchAsync(input.BranchId, input.TableNumber.Trim()))
            throw new InvalidOperationException("A table with this table number already exists in the branch.");
        var entity = _mapper.Map<Table>(input);
        return _mapper.Map<TableResponseDto>(await _repository.CreateAsync(entity));
    }

    public async Task<List<TableResponseDto>> GetAllAsync() => _mapper.Map<List<TableResponseDto>>(await _repository.GetAllAsync());

    public async Task<TableResponseDto> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Table not found.");
        return _mapper.Map<TableResponseDto>(entity);
    }

    public async Task<TableResponseDto> UpdateAsync(int id, CreateUpdateTableDto input)
    {
        var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Table not found.");
        if (await _repository.ExistsForBranchAsync(input.BranchId, input.TableNumber.Trim(), id))
            throw new InvalidOperationException("A table with this table number already exists in the branch.");
        _mapper.Map(input, entity);
        entity.UpdatedDate = DateTime.UtcNow;
        return _mapper.Map<TableResponseDto>(await _repository.UpdateAsync(entity));
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Table not found.");
        await _repository.DeleteAsync(entity);
    }
}
