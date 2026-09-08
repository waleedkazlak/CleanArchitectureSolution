using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class ClientRepository : IClientRepository
{
    private readonly CleanSampleDbContext _context;

    public ClientRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.ToListAsync();
    }

    public async Task<int> AddAsync(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client.Id;
    }

    public async Task UpdateAsync(Client client)
    {
        _context.Entry(client).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }
}
