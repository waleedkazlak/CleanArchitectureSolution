using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class LoadRequestPartRepository : ILoadRequestPartRepository
{
    private readonly CleanSampleDbContext _context;

    public LoadRequestPartRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<LoadRequestPart?> GetByIdAsync(long id)
    {
        return await _context.LoadRequestParts
            .Include(p => p.LoadRequest)
            .Include(p => p.LoadRequestLine)
            .Include(p => p.Part)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<LoadRequestPart>> GetAllAsync()
    {
        return await _context.LoadRequestParts
            .Include(p => p.LoadRequest)
            .Include(p => p.LoadRequestLine)
            .Include(p => p.Part)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoadRequestPart>> GetByLoadRequestIdAsync(long loadRequestId)
    {
        return await _context.LoadRequestParts
            .Include(p => p.LoadRequest)
            .Include(p => p.LoadRequestLine)
            .Include(p => p.Part)
            .Where(p => p.LoadRequestId == loadRequestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoadRequestPart>> GetByLoadRequestLineIdAsync(long loadRequestLineId)
    {
        return await _context.LoadRequestParts
            .Include(p => p.LoadRequest)
            .Include(p => p.LoadRequestLine)
            .Include(p => p.Part)
            .Where(p => p.LoadRequestLineId == loadRequestLineId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(LoadRequestPart loadRequestPart)
    {
        _context.LoadRequestParts.Add(loadRequestPart);
        await _context.SaveChangesAsync();
        return loadRequestPart.Id;
    }

    public async Task UpdateAsync(LoadRequestPart loadRequestPart)
    {
        _context.Entry(loadRequestPart).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var loadRequestPart = await _context.LoadRequestParts.FindAsync(id);
        if (loadRequestPart != null)
        {
            _context.LoadRequestParts.Remove(loadRequestPart);
            await _context.SaveChangesAsync();
        }
    }
}
