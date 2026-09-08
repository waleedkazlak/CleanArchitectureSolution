using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class PartRepository : IPartRepository
{
    private readonly CleanSampleDbContext _context;

    public PartRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Part?> GetByIdAsync(int id)
    {
        return await _context.Parts.FindAsync(id);
    }

    public async Task<IEnumerable<Part>> GetAllAsync()
    {
        return await _context.Parts.ToListAsync();
    }

    public async Task<int> AddAsync(Part part)
    {
        _context.Parts.Add(part);
        await _context.SaveChangesAsync();
        return part.Id;
    }

    public async Task UpdateAsync(Part part)
    {
        _context.Entry(part).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var part = await _context.Parts.FindAsync(id);
        if (part != null)
        {
            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
        }
    }
}
