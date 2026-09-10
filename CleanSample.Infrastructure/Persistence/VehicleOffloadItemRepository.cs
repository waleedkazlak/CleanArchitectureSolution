using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class VehicleOffloadItemRepository : IVehicleOffloadItemRepository
{
    private readonly CleanSampleDbContext _context;

    public VehicleOffloadItemRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleOffloadItem?> GetByIdAsync(long id)
    {
        return await _context.VehicleOffloadItems
            .Include(v => v.VehicleOffload)
            .Include(v => v.Part)
            .Include(v => v.Load)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<VehicleOffloadItem>> GetAllAsync()
    {
        return await _context.VehicleOffloadItems
            .Include(v => v.VehicleOffload)
            .Include(v => v.Part)
            .Include(v => v.Load)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleOffloadItem>> GetByVehicleOffloadIdAsync(long vehicleOffloadId)
    {
        return await _context.VehicleOffloadItems
            .Include(v => v.VehicleOffload)
            .Include(v => v.Part)
            .Include(v => v.Load)
            .Where(v => v.VehicleOffloadId == vehicleOffloadId)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleOffloadItem>> GetByLoadIdAsync(long loadId)
    {
        return await _context.VehicleOffloadItems
            .Include(v => v.VehicleOffload)
            .Include(v => v.Part)
            .Include(v => v.Load)
            .Where(v => v.LoadId == loadId)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleOffloadItem>> GetByBarcodeAsync(string barcode)
    {
        return await _context.VehicleOffloadItems
            .Include(v => v.VehicleOffload)
            .Include(v => v.Part)
            .Include(v => v.Load)
            .Where(v => v.Barcode == barcode)
            .ToListAsync();
    }

    public async Task<long> AddAsync(VehicleOffloadItem item)
    {
        _context.VehicleOffloadItems.Add(item);
        await _context.SaveChangesAsync();
        return item.Id;
    }

    public async Task<List<VehicleOffloadItem>> AddRangeAsync(IEnumerable<VehicleOffloadItem> items)
    {
        var itemList = items.ToList();
        _context.VehicleOffloadItems.AddRange(itemList);
        await _context.SaveChangesAsync();
        return itemList;
    }

    public async Task UpdateAsync(VehicleOffloadItem item)
    {
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var item = await _context.VehicleOffloadItems.FindAsync(id);
        if (item != null)
        {
            _context.VehicleOffloadItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
