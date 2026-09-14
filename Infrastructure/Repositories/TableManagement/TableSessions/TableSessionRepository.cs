using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.TableSessions;

public class TableSessionRepository : ITableSessionRepository
{
    private readonly DataContext _context;

    public TableSessionRepository(DataContext context) => _context = context;

    public async Task<TableSession> CreateAsync(TableSession session)
    {
        _context.TableSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public Task<List<TableSession>> GetAllAsync() => _context.TableSessions.ToListAsync();

    public Task<TableSession?> GetByIdAsync(int id) =>
        _context.TableSessions.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<TableSession> UpdateAsync(TableSession session)
    {
        session.UpdatedDate = DateTime.UtcNow;
        _context.TableSessions.Update(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task DeleteAsync(TableSession session)
    {
        session.IsDeleted = true;
        session.IsActive = false;
        session.UpdatedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}
