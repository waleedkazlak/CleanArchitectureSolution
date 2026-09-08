using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class PickRequestLineRepository : IPickRequestLineRepository
{
    private readonly CleanSampleDbContext _context;

    public PickRequestLineRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<PickRequestLine?> GetByIdAsync(long id)
    {
        return await _context.PickRequestLines
            .Include(p => p.PickRequest)
            .Include(p => p.ProductVariant)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<PickRequestLine>> GetAllAsync()
    {
        return await _context.PickRequestLines
            .Include(p => p.PickRequest)
            .Include(p => p.ProductVariant)
            .ToListAsync();
    }

    public async Task<IEnumerable<PickRequestLine>> GetByPickRequestIdAsync(long pickRequestId)
    {
        return await _context.PickRequestLines
            .Include(p => p.PickRequest)
            .Include(p => p.ProductVariant)
            .Where(p => p.PickRequestId == pickRequestId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(PickRequestLine pickRequestLine)
    {
        _context.PickRequestLines.Add(pickRequestLine);
        await _context.SaveChangesAsync();
        return pickRequestLine.Id;
    }

    public async Task UpdateAsync(PickRequestLine pickRequestLine)
    {
        _context.Entry(pickRequestLine).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var pickRequestLine = await _context.PickRequestLines.FindAsync(id);
        if (pickRequestLine != null)
        {
            _context.PickRequestLines.Remove(pickRequestLine);
            await _context.SaveChangesAsync();
        }
    }
}
