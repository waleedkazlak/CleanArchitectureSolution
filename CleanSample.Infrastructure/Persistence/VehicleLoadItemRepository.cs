using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class VehicleLoadItemRepository : IVehicleLoadItemRepository
{
    private readonly CleanSampleDbContext _context;

    public VehicleLoadItemRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<VehicleLoadItem?> GetByIdAsync(long id)
    {
        return await _context.VehicleLoadItems
            .Include(v => v.VehicleLoad)
            .Include(v => v.Part)
            .Include(v => v.Pick)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<VehicleLoadItem>> GetAllAsync()
    {
        return await _context.VehicleLoadItems
            .Include(v => v.VehicleLoad)
            .Include(v => v.Part)
            .Include(v => v.Pick)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleLoadItem>> GetByVehicleLoadIdAsync(long vehicleLoadId)
    {
        return await _context.VehicleLoadItems
            .Include(v => v.VehicleLoad)
            .Include(v => v.Part)
            .Include(v => v.Pick)
            .Where(v => v.VehicleLoadId == vehicleLoadId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(VehicleLoadItem vehicleLoadItem)
    {
        _context.VehicleLoadItems.Add(vehicleLoadItem);
        await _context.SaveChangesAsync();
        return vehicleLoadItem.Id;
    }

    public async Task<IEnumerable<long>> AddRangeAsync(IEnumerable<VehicleLoadItem> items)
    {
        var itemList = items.ToList();
        _context.VehicleLoadItems.AddRange(itemList);
        await _context.SaveChangesAsync();
        return itemList.Select(i => i.Id).ToList();
    }

    public async Task UpdateAsync(VehicleLoadItem vehicleLoadItem)
    {
        _context.Entry(vehicleLoadItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var item = await _context.VehicleLoadItems.FindAsync(id);
        if (item != null)
        {
            _context.VehicleLoadItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteRangeAsync(IEnumerable<long> ids)
    {
        var idList = ids.ToList();
        var items = await _context.VehicleLoadItems.Where(i => idList.Contains(i.Id)).ToListAsync();
        if (items.Any())
        {
            _context.VehicleLoadItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
