using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class ClientLocationRepository : IClientLocationRepository
{
    private readonly CleanSampleDbContext _context;

    public ClientLocationRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<ClientLocation?> GetByIdAsync(int id)
    {
        return await _context.ClientLocations
            .Include(cl => cl.Client)
            .FirstOrDefaultAsync(cl => cl.Id == id);
    }

    public async Task<IEnumerable<ClientLocation>> GetAllAsync()
    {
        return await _context.ClientLocations
            .Include(cl => cl.Client)
            .ToListAsync();
    }

    public async Task<IEnumerable<ClientLocation>> GetByClientIdAsync(int clientId)
    {
        return await _context.ClientLocations
            .Include(cl => cl.Client)
            .Where(cl => cl.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<ClientLocation?> GetByNameAndClientIdAsync(int clientId, string name)
    {
        return await _context.ClientLocations
            .Include(cl => cl.Client)
            .FirstOrDefaultAsync(cl => cl.ClientId == clientId && cl.Name == name);
    }

    public async Task<int> AddAsync(ClientLocation clientLocation)
    {
        _context.ClientLocations.Add(clientLocation);
        await _context.SaveChangesAsync();
        return clientLocation.Id;
    }

    public async Task<List<ClientLocation>> AddRangeAsync(IEnumerable<ClientLocation> clientLocations)
    {
        var locationList = clientLocations.ToList();
        _context.ClientLocations.AddRange(locationList);
        await _context.SaveChangesAsync();
        return locationList;
    }

    public async Task UpdateAsync(ClientLocation clientLocation)
    {
        _context.Entry(clientLocation).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<ClientLocation>> UpdateRangeAsync(IEnumerable<ClientLocation> clientLocations)
    {
        var locationList = clientLocations.ToList();
        foreach (var location in locationList)
        {
            _context.Entry(location).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
        return locationList;
    }

    public async Task DeleteAsync(int id)
    {
        var clientLocation = await _context.ClientLocations.FindAsync(id);
        if (clientLocation != null)
        {
            _context.ClientLocations.Remove(clientLocation);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
    {
        var idList = ids.ToList();
        var locations = await _context.ClientLocations.Where(cl => idList.Contains(cl.Id)).ToListAsync();
        if (locations.Any())
        {
            _context.ClientLocations.RemoveRange(locations);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
