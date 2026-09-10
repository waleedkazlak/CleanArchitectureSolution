using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class LoadRepository : ILoadRepository
{
    private readonly CleanSampleDbContext _context;

    public LoadRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Load?> GetByIdAsync(long id)
    {
        return await _context.Loads
            .Include(l => l.LoadRequest)
            .Include(l => l.LoadRequestPart)
            .Include(l => l.Part)
            .Include(l => l.Loader)
            .Include(l => l.Driver)
            .Include(l => l.Vehicle)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Load>> GetAllAsync()
    {
        return await _context.Loads
            .Include(l => l.LoadRequest)
            .Include(l => l.LoadRequestPart)
            .Include(l => l.Part)
            .Include(l => l.Loader)
            .Include(l => l.Driver)
            .Include(l => l.Vehicle)
            .ToListAsync();
    }

    public async Task<IEnumerable<Load>> GetByLoadRequestIdAsync(long loadRequestId)
    {
        return await _context.Loads
            .Include(l => l.LoadRequest)
            .Include(l => l.LoadRequestPart)
            .Include(l => l.Part)
            .Include(l => l.Loader)
            .Include(l => l.Driver)
            .Include(l => l.Vehicle)
            .Where(l => l.LoadRequestId == loadRequestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Load>> GetByDriverIdAsync(int driverId)
    {
        return await _context.Loads
            .Include(l => l.LoadRequest)
            .Include(l => l.LoadRequestPart)
            .Include(l => l.Part)
            .Include(l => l.Loader)
            .Include(l => l.Driver)
            .Include(l => l.Vehicle)
            .Where(l => l.DriverId == driverId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Load>> GetByBarcodeAsync(string barcode)
    {
        return await _context.Loads
            .Include(l => l.LoadRequest)
            .Include(l => l.LoadRequestPart)
            .Include(l => l.Part)
            .Include(l => l.Loader)
            .Include(l => l.Driver)
            .Include(l => l.Vehicle)
            .Where(l => l.Barcode == barcode)
            .ToListAsync();
    }

    public async Task<long> AddAsync(Load load)
    {
        _context.Loads.Add(load);
        await _context.SaveChangesAsync();
        return load.Id;
    }

    public async Task<List<Load>> AddRangeAsync(IEnumerable<Load> loads)
    {
        var loadList = loads.ToList();
        _context.Loads.AddRange(loadList);
        await _context.SaveChangesAsync();
        return loadList;
    }

    public async Task UpdateAsync(Load load)
    {
        _context.Entry(load).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Load>> UpdateRangeAsync(IEnumerable<Load> loads)
    {
        var loadList = loads.ToList();
        foreach (var load in loadList)
        {
            _context.Entry(load).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
        return loadList;
    }

    public async Task DeleteAsync(long id)
    {
        var load = await _context.Loads.FindAsync(id);
        if (load != null)
        {
            _context.Loads.Remove(load);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<long> ids)
    {
        var idList = ids.ToList();
        var loads = await _context.Loads.Where(l => idList.Contains(l.Id)).ToListAsync();
        if (loads.Any())
        {
            _context.Loads.RemoveRange(loads);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
