using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class VehicleOffloadRepository : IVehicleOffloadRepository
{
    private readonly CleanSampleDbContext _context;

    public VehicleOffloadRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleOffload?> GetByIdAsync(long id)
    {
        return await _context.VehicleOffloads
            .Include(v => v.LoadRequest)
            .Include(v => v.Part)
            .Include(v => v.Vehicle)
            .Include(v => v.Driver)
            .Include(v => v.Verifier)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<VehicleOffload>> GetAllAsync()
    {
        return await _context.VehicleOffloads
            .Include(v => v.LoadRequest)
            .Include(v => v.Part)
            .Include(v => v.Vehicle)
            .Include(v => v.Driver)
            .Include(v => v.Verifier)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleOffload>> GetByLoadRequestIdAsync(long loadRequestId)
    {
        return await _context.VehicleOffloads
            .Include(v => v.LoadRequest)
            .Include(v => v.Part)
            .Include(v => v.Vehicle)
            .Include(v => v.Driver)
            .Include(v => v.Verifier)
            .Where(v => v.LoadRequestId == loadRequestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleOffload>> GetByDriverIdAsync(int driverId)
    {
        return await _context.VehicleOffloads
            .Include(v => v.LoadRequest)
            .Include(v => v.Part)
            .Include(v => v.Vehicle)
            .Include(v => v.Driver)
            .Include(v => v.Verifier)
            .Where(v => v.DriverId == driverId)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleOffload>> GetByVehicleIdAsync(int vehicleId)
    {
        return await _context.VehicleOffloads
            .Include(v => v.LoadRequest)
            .Include(v => v.Part)
            .Include(v => v.Vehicle)
            .Include(v => v.Driver)
            .Include(v => v.Verifier)
            .Where(v => v.VehicleId == vehicleId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(VehicleOffload vehicleOffload)
    {
        _context.VehicleOffloads.Add(vehicleOffload);
        await _context.SaveChangesAsync();
        return vehicleOffload.Id;
    }

    public async Task AddRangeAsync(IEnumerable<VehicleOffload> vehicleOffloads)
    {
        await _context.VehicleOffloads.AddRangeAsync(vehicleOffloads);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(VehicleOffload vehicleOffload)
    {
        _context.Entry(vehicleOffload).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var vehicleOffload = await _context.VehicleOffloads
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vehicleOffload != null)
        {
            _context.VehicleOffloads.Remove(vehicleOffload);
            await _context.SaveChangesAsync();
        }
    }
}
