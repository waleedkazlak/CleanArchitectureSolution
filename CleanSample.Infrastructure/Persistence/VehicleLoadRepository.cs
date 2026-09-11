using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class VehicleLoadRepository : IVehicleLoadRepository
{
    private readonly CleanSampleDbContext _context;

    public VehicleLoadRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleLoad?> GetByIdAsync(long id)
    {
        return await _context.VehicleLoads
            .Include(l => l.LoadRequest)
            .Include(l => l.Part)
            .Include(l => l.Loader)
            .Include(l => l.Driver)
            .Include(l => l.Vehicle)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<VehicleLoad>> GetAllAsync()
    {
        return await _context.VehicleLoads
            .Include(l => l.LoadRequest)
            .Include(l => l.Part)
            .Include(l => l.Loader)
            .Include(l => l.Driver)
            .Include(l => l.Vehicle)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleLoad>> GetByLoadRequestIdAsync(long loadRequestId)
    {
        return await _context.VehicleLoads
            .Include(l => l.LoadRequest)
            .Include(l => l.Part)
            .Include(l => l.Loader)
            .Include(l => l.Driver)
            .Include(l => l.Vehicle)
            .Where(l => l.LoadRequestId == loadRequestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleLoad>> GetByDriverIdAsync(int driverId)
    {
        return await _context.VehicleLoads
            .Include(l => l.LoadRequest)
            .Include(l => l.Part)
            .Include(l => l.Loader)
            .Include(l => l.Driver)
            .Include(l => l.Vehicle)
            .Where(l => l.DriverId == driverId)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleLoad>> GetByBarcodeAsync(string barcode)
    {
        return await _context.VehicleLoads
            .Include(l => l.LoadRequest)
            .Include(l => l.Part)
            .Include(l => l.Loader)
            .Include(l => l.Driver)
            .Include(l => l.Vehicle)
            .Where(l => l.Barcode == barcode)
            .ToListAsync();
    }

    public async Task<long> AddAsync(VehicleLoad vehicleLoad)
    {
        _context.VehicleLoads.Add(vehicleLoad);
        await _context.SaveChangesAsync();
        return vehicleLoad.Id;
    }

    public async Task<List<VehicleLoad>> AddRangeAsync(IEnumerable<VehicleLoad> vehicleLoads)
    {
        var list = vehicleLoads.ToList();
        await _context.VehicleLoads.AddRangeAsync(list);
        await _context.SaveChangesAsync();
        return list;
    }

    public async Task UpdateAsync(VehicleLoad vehicleLoad)
    {
        _context.Entry(vehicleLoad).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<VehicleLoad>> UpdateRangeAsync(IEnumerable<VehicleLoad> vehicleLoads)
    {
        var list = vehicleLoads.ToList();
        _context.VehicleLoads.UpdateRange(list);
        await _context.SaveChangesAsync();
        return list;
    }

    public async Task DeleteAsync(long id)
    {
        var vehicleLoad = await _context.VehicleLoads.FindAsync(id);
        if (vehicleLoad != null)
        {
            _context.VehicleLoads.Remove(vehicleLoad);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<long> ids)
    {
        var idList = ids.ToList();
        var loadsToDelete = await _context.VehicleLoads
            .Where(l => idList.Contains(l.Id))
            .ToListAsync();

        if (!loadsToDelete.Any())
        {
            return false;
        }

        _context.VehicleLoads.RemoveRange(loadsToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
}
