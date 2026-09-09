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
            .Include(v => v.PickRequest)
            .Include(v => v.Vehicle)
            .Include(v => v.Driver)
            .Include(v => v.Verifier)
            .Include(v => v.VehicleLoadItems)
                .ThenInclude(i => i.Part)
            .Include(v => v.VehicleLoadItems)
                .ThenInclude(i => i.Pick)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<VehicleLoad>> GetAllAsync()
    {
        return await _context.VehicleLoads
            .Include(v => v.PickRequest)
            .Include(v => v.Vehicle)
            .Include(v => v.Driver)
            .Include(v => v.Verifier)
            .Include(v => v.VehicleLoadItems)
                .ThenInclude(i => i.Part)
            .Include(v => v.VehicleLoadItems)
                .ThenInclude(i => i.Pick)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleLoad>> GetByPickRequestIdAsync(long pickRequestId)
    {
        return await _context.VehicleLoads
            .Include(v => v.PickRequest)
            .Include(v => v.Vehicle)
            .Include(v => v.Driver)
            .Include(v => v.Verifier)
            .Include(v => v.VehicleLoadItems)
                .ThenInclude(i => i.Part)
            .Include(v => v.VehicleLoadItems)
                .ThenInclude(i => i.Pick)
            .Where(v => v.PickRequestId == pickRequestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleLoad>> GetByDriverIdAsync(int driverId)
    {
        return await _context.VehicleLoads
            .Include(v => v.PickRequest)
            .Include(v => v.Vehicle)
            .Include(v => v.Driver)
            .Include(v => v.Verifier)
            .Include(v => v.VehicleLoadItems)
                .ThenInclude(i => i.Part)
            .Include(v => v.VehicleLoadItems)
                .ThenInclude(i => i.Pick)
            .Where(v => v.DriverId == driverId)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleLoad>> GetByVehicleIdAsync(int vehicleId)
    {
        return await _context.VehicleLoads
            .Include(v => v.PickRequest)
            .Include(v => v.Vehicle)
            .Include(v => v.Driver)
            .Include(v => v.Verifier)
            .Include(v => v.VehicleLoadItems)
                .ThenInclude(i => i.Part)
            .Include(v => v.VehicleLoadItems)
                .ThenInclude(i => i.Pick)
            .Where(v => v.VehicleId == vehicleId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(VehicleLoad vehicleLoad)
    {
        _context.VehicleLoads.Add(vehicleLoad);
        await _context.SaveChangesAsync();
        return vehicleLoad.Id;
    }

    public async Task UpdateAsync(VehicleLoad vehicleLoad)
    {
        _context.Entry(vehicleLoad).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var vehicleLoad = await _context.VehicleLoads
            .Include(v => v.VehicleLoadItems)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vehicleLoad != null)
        {
            _context.VehicleLoads.Remove(vehicleLoad);
            await _context.SaveChangesAsync();
        }
    }
}
