using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class DesignRepository : IDesignRepository
{
    private readonly CleanSampleDbContext _context;

    public DesignRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Design?> GetByIdAsync(int id)
    {
        return await _context.Designs.FindAsync(id);
    }

    public async Task<IEnumerable<Design>> GetAllAsync()
    {
        return await _context.Designs.ToListAsync();
    }

    public async Task<int> AddAsync(Design design)
    {
        _context.Designs.Add(design);
        await _context.SaveChangesAsync();
        return design.Id;
    }

    public async Task UpdateAsync(Design design)
    {
        _context.Entry(design).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var design = await _context.Designs.FindAsync(id);
        if (design != null)
        {
            _context.Designs.Remove(design);
            await _context.SaveChangesAsync();
        }
    }
}
