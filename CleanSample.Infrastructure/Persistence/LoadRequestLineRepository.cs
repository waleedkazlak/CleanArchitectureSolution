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

    public IQueryable<LoadRequestLine> GetQueryable()
    {
        return _context.LoadRequestLines
            .AsNoTracking()
            .Include(l => l.LoadRequest)
                .ThenInclude(lr => lr.Order)
            .Include(l => l.LoadRequest)
                .ThenInclude(lr => lr.Driver)
            .Include(l => l.Product)
            .Include(l => l.LoadRequestParts)
                .ThenInclude(lrp => lrp.Part)
            .AsQueryable();
    }

    public async Task<LoadRequestLine?> GetByIdAsync(long id)
    {
        return await _context.LoadRequestLines
            .Include(p => p.LoadRequest)
            .Include(p => p.Product)
            .Include(p => p.LoadRequestParts)
                .ThenInclude(prp => prp.Part)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<LoadRequestLine>> GetAllAsync()
    {
        return await _context.LoadRequestLines
            .Include(p => p.LoadRequest)
            .Include(p => p.Product)
            .Include(p => p.LoadRequestParts)
                .ThenInclude(prp => prp.Part)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoadRequestLine>> GetByLoadRequestIdAsync(long loadRequestId)
    {
        return await _context.LoadRequestLines
            .Include(p => p.LoadRequest)
            .Include(p => p.Product)
            .Include(p => p.LoadRequestParts)
                .ThenInclude(prp => prp.Part)
            .Where(p => p.LoadRequestId == loadRequestId)
            .ToListAsync();
    }

    public async Task<LoadRequestLine?> GetByLoadRequestAndProductIdAsync(long loadRequestId, int productId)
    {
        return await _context.LoadRequestLines
            .Include(p => p.LoadRequest)
            .Include(p => p.Product)
            .Include(p => p.LoadRequestParts)
                .ThenInclude(prp => prp.Part)
            .FirstOrDefaultAsync(p => p.LoadRequestId == loadRequestId && p.ProductId == productId);
    }

    public async Task<long> AddAsync(LoadRequestLine loadRequestLine)
    {
        _context.LoadRequestLines.Add(loadRequestLine);
        await _context.SaveChangesAsync();
        return loadRequestLine.Id;
    }

    public async Task<List<LoadRequestLine>> AddRangeAsync(IEnumerable<LoadRequestLine> loadRequestLines)
    {
        var lineList = loadRequestLines.ToList();
        _context.LoadRequestLines.AddRange(lineList);
        await _context.SaveChangesAsync();
        return lineList;
    }

    public async Task UpdateAsync(LoadRequestLine loadRequestLine)
    {
        _context.Entry(loadRequestLine).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<LoadRequestLine>> UpdateRangeAsync(IEnumerable<LoadRequestLine> loadRequestLines)
    {
        var lineList = loadRequestLines.ToList();
        foreach (var line in lineList)
        {
            _context.Entry(line).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
        return lineList;
    }

    public async Task DeleteAsync(long id)
    {
        var loadRequestLine = await _context.LoadRequestLines
            .Include(p => p.LoadRequestParts)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (loadRequestLine != null)
        {
            _context.LoadRequestLines.Remove(loadRequestLine);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<long> ids)
    {
        var idList = ids.ToList();
        var lines = await _context.LoadRequestLines
            .Include(p => p.LoadRequestParts)
            .Where(l => idList.Contains(l.Id))
            .ToListAsync();

        if (lines.Any())
        {
            _context.LoadRequestLines.RemoveRange(lines);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
