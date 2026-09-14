using Application.Dtos.TableSessions;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.TableSessions;

namespace Application.Applications.TableSessions;

public class TableSessionApplication : ITableSessionApplication
{
    private readonly ITableSessionRepository _tableSessionRepository;
    private readonly IMapper _mapper;

    public TableSessionApplication(
        ITableSessionRepository tableSessionRepository,
        IMapper mapper)
    {
        _tableSessionRepository = tableSessionRepository;
        _mapper = mapper;
    }

    public async Task<TableSessionResponseDto> CreateAsync(
        CreateUpdateTableSessionDto input)
    {
        var session = _mapper.Map<TableSession>(input);

        session.StartTime = input.StartTime ?? DateTime.UtcNow;
        session.IsActive = true;
        session.IsDeleted = false;

        var result = await _tableSessionRepository.CreateAsync(session);

        return _mapper.Map<TableSessionResponseDto>(result);
    }

    public async Task<List<TableSessionResponseDto>> GetAllAsync()
    {
        var sessions = await _tableSessionRepository.GetAllAsync();

        return _mapper.Map<List<TableSessionResponseDto>>(sessions);
    }

    public async Task<TableSessionResponseDto?> GetByIdAsync(int id)
    {
        var session = await _tableSessionRepository.GetByIdAsync(id);

        if (session == null)
            return null;

        return _mapper.Map<TableSessionResponseDto>(session);
    }

    public async Task<TableSessionResponseDto?> UpdateAsync(
        int id,
        CreateUpdateTableSessionDto input)
    {
        var session = await _tableSessionRepository.GetByIdAsync(id);

        if (session == null)
            return null;

        _mapper.Map(input, session);

        session.StartTime = input.StartTime ?? session.StartTime;

        session.IsActive = input.EndTime == null;

        var result = await _tableSessionRepository.UpdateAsync(session);

        return _mapper.Map<TableSessionResponseDto>(result);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var session = await _tableSessionRepository.GetByIdAsync(id);

        if (session == null)
            return false;

        await _tableSessionRepository.DeleteAsync(session);

        return true;
    }
}