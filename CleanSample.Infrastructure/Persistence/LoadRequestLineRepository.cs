using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class LoadRequestLineRepository : ILoadRequestLineRepository
{
    private readonly CleanSampleDbContext _context;

    public LoadRequestLineRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<LoadRequestLine?> GetByIdAsync(long id)
    {
        return await _context.LoadRequestLines
            .Include(p => p.LoadRequest)
            .Include(p => p.ProductVariant)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<LoadRequestLine>> GetAllAsync()
    {
        return await _context.LoadRequestLines
            .Include(p => p.LoadRequest)
            .Include(p => p.ProductVariant)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoadRequestLine>> GetByLoadRequestIdAsync(long loadRequestId)
    {
        return await _context.LoadRequestLines
            .Include(p => p.LoadRequest)
            .Include(p => p.ProductVariant)
            .Where(p => p.LoadRequestId == loadRequestId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(LoadRequestLine loadRequestLine)
    {
        _context.LoadRequestLines.Add(loadRequestLine);
        await _context.SaveChangesAsync();
        return loadRequestLine.Id;
    }

    public async Task UpdateAsync(LoadRequestLine loadRequestLine)
    {
        _context.Entry(loadRequestLine).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var loadRequestLine = await _context.LoadRequestLines.FindAsync(id);
        if (loadRequestLine != null)
        {
            _context.LoadRequestLines.Remove(loadRequestLine);
            await _context.SaveChangesAsync();
        }
    }
}
