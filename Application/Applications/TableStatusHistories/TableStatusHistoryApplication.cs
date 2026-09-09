using Application.Dtos.TableStatusHistories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.TableStatusHistories;

namespace Application.Applications.TableStatusHistories;

public class TableStatusHistoryApplication : ITableStatusHistoryApplication
{
    private readonly ITableStatusHistoryRepository _repository;
    private readonly IMapper _mapper;
    public TableStatusHistoryApplication(ITableStatusHistoryRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }

    public async Task<TableStatusHistoryResponseDto> CreateAsync(CreateUpdateTableStatusHistoryDto input)
    {
        var entity = _mapper.Map<TableStatusHistory>(input);
        if (entity.StartTime == default) entity.StartTime = DateTime.UtcNow;
        return _mapper.Map<TableStatusHistoryResponseDto>(await _repository.CreateAsync(entity));
    }
    public async Task<List<TableStatusHistoryResponseDto>> GetAllAsync() => _mapper.Map<List<TableStatusHistoryResponseDto>>(await _repository.GetAllAsync());
    public async Task<TableStatusHistoryResponseDto> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Table status history not found.");
        return _mapper.Map<TableStatusHistoryResponseDto>(entity);
    }
    public async Task<TableStatusHistoryResponseDto> UpdateAsync(int id, CreateUpdateTableStatusHistoryDto input)
    {
        var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Table status history not found.");
        _mapper.Map(input, entity);
        entity.UpdatedDate = DateTime.UtcNow;
        return _mapper.Map<TableStatusHistoryResponseDto>(await _repository.UpdateAsync(entity));
    }
    public async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Table status history not found.");
        await _repository.DeleteAsync(entity);
    }
}
