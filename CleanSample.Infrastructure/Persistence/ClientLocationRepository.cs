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

    public async Task<int> AddAsync(ClientLocation clientLocation)
    {
        _context.ClientLocations.Add(clientLocation);
        await _context.SaveChangesAsync();
        return clientLocation.Id;
    }

    public async Task UpdateAsync(ClientLocation clientLocation)
    {
        _context.Entry(clientLocation).State = EntityState.Modified;
        await _context.SaveChangesAsync();
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
}
