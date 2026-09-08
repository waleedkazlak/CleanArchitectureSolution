using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class MaterialRepository : IMaterialRepository
{
    private readonly CleanSampleDbContext _context;

    public MaterialRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Material?> GetByIdAsync(int id)
    {
        return await _context.Materials.FindAsync(id);
    }

    public async Task<IEnumerable<Material>> GetAllAsync()
    {
        return await _context.Materials.ToListAsync();
    }

    public async Task<int> AddAsync(Material material)
    {
        _context.Materials.Add(material);
        await _context.SaveChangesAsync();
        return material.Id;
    }

    public async Task UpdateAsync(Material material)
    {
        _context.Entry(material).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var material = await _context.Materials.FindAsync(id);
        if (material != null)
        {
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
        }
    }
}
