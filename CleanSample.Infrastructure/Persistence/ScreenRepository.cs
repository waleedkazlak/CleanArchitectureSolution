using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class ScreenRepository : IScreenRepository
{
    private readonly CleanSampleDbContext _context;

    public ScreenRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Screen?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Screens
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Screen?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Screens
            .FirstOrDefaultAsync(s => s.Code == code, cancellationToken);
    }

    public async Task<Screen?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Screens
            .FirstOrDefaultAsync(s => s.NameEn.ToLower() == name.ToLower() || (s.NameAr != null && s.NameAr.ToLower() == name.ToLower()), cancellationToken);
    }

    public async Task<IReadOnlyList<Screen>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Screens
            .OrderBy(s => s.Module)
            .ThenBy(s => s.NameEn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Screen>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Screens
            .Where(s => s.IsActive)
            .OrderBy(s => s.Module)
            .ThenBy(s => s.NameEn)
            .ToListAsync(cancellationToken);
    }

    public async Task<Screen> AddAsync(Screen screen, CancellationToken cancellationToken = default)
    {
        await _context.Screens.AddAsync(screen, cancellationToken);
        return screen;
    }

    public Task UpdateAsync(Screen screen, CancellationToken cancellationToken = default)
    {
        _context.Screens.Update(screen);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Screen screen, CancellationToken cancellationToken = default)
    {
        _context.Screens.Remove(screen);
        return Task.CompletedTask;
    }
}
