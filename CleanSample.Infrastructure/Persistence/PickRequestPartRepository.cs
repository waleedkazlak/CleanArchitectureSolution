using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class PickRequestPartRepository : IPickRequestPartRepository
{
    private readonly CleanSampleDbContext _context;

    public PickRequestPartRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<PickRequestPart?> GetByIdAsync(long id)
    {
        return await _context.PickRequestParts
            .Include(p => p.PickRequest)
            .Include(p => p.PickRequestLine)
            .Include(p => p.Part)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<PickRequestPart>> GetAllAsync()
    {
        return await _context.PickRequestParts
            .Include(p => p.PickRequest)
            .Include(p => p.PickRequestLine)
            .Include(p => p.Part)
            .ToListAsync();
    }

    public async Task<IEnumerable<PickRequestPart>> GetByPickRequestIdAsync(long pickRequestId)
    {
        return await _context.PickRequestParts
            .Include(p => p.PickRequest)
            .Include(p => p.PickRequestLine)
            .Include(p => p.Part)
            .Where(p => p.PickRequestId == pickRequestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<PickRequestPart>> GetByPickRequestLineIdAsync(long pickRequestLineId)
    {
        return await _context.PickRequestParts
            .Include(p => p.PickRequest)
            .Include(p => p.PickRequestLine)
            .Include(p => p.Part)
            .Where(p => p.PickRequestLineId == pickRequestLineId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(PickRequestPart pickRequestPart)
    {
        _context.PickRequestParts.Add(pickRequestPart);
        await _context.SaveChangesAsync();
        return pickRequestPart.Id;
    }

    public async Task UpdateAsync(PickRequestPart pickRequestPart)
    {
        _context.Entry(pickRequestPart).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var pickRequestPart = await _context.PickRequestParts.FindAsync(id);
        if (pickRequestPart != null)
        {
            _context.PickRequestParts.Remove(pickRequestPart);
            await _context.SaveChangesAsync();
        }
    }
}
