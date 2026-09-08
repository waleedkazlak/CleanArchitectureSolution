using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class PickRepository : IPickRepository
{
    private readonly CleanSampleDbContext _context;

    public PickRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Pick?> GetByIdAsync(long id)
    {
        return await _context.Picks
            .Include(p => p.PickRequest)
            .Include(p => p.PickRequestPart)
            .Include(p => p.Part)
            .Include(p => p.Picker)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Pick>> GetAllAsync()
    {
        return await _context.Picks
            .Include(p => p.PickRequest)
            .Include(p => p.PickRequestPart)
            .Include(p => p.Part)
            .Include(p => p.Picker)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pick>> GetByPickRequestIdAsync(long pickRequestId)
    {
        return await _context.Picks
            .Include(p => p.PickRequest)
            .Include(p => p.PickRequestPart)
            .Include(p => p.Part)
            .Include(p => p.Picker)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Where(p => p.PickRequestId == pickRequestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pick>> GetByPickRequestPartIdAsync(long pickRequestPartId)
    {
        return await _context.Picks
            .Include(p => p.PickRequest)
            .Include(p => p.PickRequestPart)
            .Include(p => p.Part)
            .Include(p => p.Picker)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Where(p => p.PickRequestPartId == pickRequestPartId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pick>> GetByDriverIdAsync(int driverId)
    {
        return await _context.Picks
            .Include(p => p.PickRequest)
            .Include(p => p.PickRequestPart)
            .Include(p => p.Part)
            .Include(p => p.Picker)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Where(p => p.DriverId == driverId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Pick>> GetByPickedByAsync(int pickedBy)
    {
        return await _context.Picks
            .Include(p => p.PickRequest)
            .Include(p => p.PickRequestPart)
            .Include(p => p.Part)
            .Include(p => p.Picker)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Where(p => p.PickedBy == pickedBy)
            .ToListAsync();
    }

    public async Task<long> AddAsync(Pick pick)
    {
        _context.Picks.Add(pick);
        await _context.SaveChangesAsync();
        return pick.Id;
    }

    public async Task<IEnumerable<long>> AddRangeAsync(IEnumerable<Pick> picks)
    {
        var pickList = picks.ToList();
        _context.Picks.AddRange(pickList);
        await _context.SaveChangesAsync();
        return pickList.Select(p => p.Id).ToList();
    }

    public async Task UpdateAsync(Pick pick)
    {
        _context.Entry(pick).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<Pick> picks)
    {
        foreach (var pick in picks)
        {
            _context.Entry(pick).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var pick = await _context.Picks.FindAsync(id);
        if (pick != null)
        {
            _context.Picks.Remove(pick);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteRangeAsync(IEnumerable<long> ids)
    {
        var idList = ids.ToList();
        var picks = await _context.Picks.Where(p => idList.Contains(p.Id)).ToListAsync();
        if (picks.Any())
        {
            _context.Picks.RemoveRange(picks);
            await _context.SaveChangesAsync();
        }
    }
}
