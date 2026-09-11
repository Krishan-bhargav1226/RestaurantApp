using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PasswordResetOTPs;

public class PasswordResetOTPRepository : IPasswordResetOTPRepository
{
    private readonly DataContext _context;

    public PasswordResetOTPRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<PasswordResetOTP> CreateAsync(PasswordResetOTP passwordResetOTP)
    {
        _context.PasswordResetOTPs.Add(passwordResetOTP);
        await _context.SaveChangesAsync();
        return passwordResetOTP;
    }

    public async Task<List<PasswordResetOTP>> GetAllAsync()
    {
        return await _context.PasswordResetOTPs
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<PasswordResetOTP?> GetByIdAsync(int id)
    {
        return await _context.PasswordResetOTPs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<PasswordResetOTP> UpdateAsync(PasswordResetOTP passwordResetOTP)
    {
        _context.PasswordResetOTPs.Update(passwordResetOTP);
        await _context.SaveChangesAsync();
        return passwordResetOTP;
    }

    public async Task DeleteAsync(PasswordResetOTP passwordResetOTP)
    {
        passwordResetOTP.IsDeleted = true;
        passwordResetOTP.UpdatedDate = DateTime.UtcNow;
        _context.PasswordResetOTPs.Update(passwordResetOTP);
        await _context.SaveChangesAsync();
    }
}