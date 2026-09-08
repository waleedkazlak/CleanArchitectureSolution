using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class VehicleRepository : IVehicleRepository
{
    private readonly CleanSampleDbContext _context;

    public VehicleRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle?> GetByIdAsync(int id)
    {
        return await _context.Vehicles.FindAsync(id);
    }

    public async Task<IEnumerable<Vehicle>> GetAllAsync()
    {
        return await _context.Vehicles.ToListAsync();
    }

    public async Task<int> AddAsync(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
        return vehicle.Id;
    }

    public async Task UpdateAsync(Vehicle vehicle)
    {
        _context.Entry(vehicle).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle != null)
        {
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }
    }
}
