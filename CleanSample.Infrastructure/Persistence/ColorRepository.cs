using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class ColorRepository : IColorRepository
{
    private readonly CleanSampleDbContext _context;

    public ColorRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Color?> GetByIdAsync(int id)
    {
        return await _context.Colors.FindAsync(id);
    }

    public async Task<IEnumerable<Color>> GetAllAsync()
    {
        return await _context.Colors.ToListAsync();
    }

    public async Task<int> AddAsync(Color color)
    {
        _context.Colors.Add(color);
        await _context.SaveChangesAsync();
        return color.Id;
    }

    public async Task UpdateAsync(Color color)
    {
        _context.Entry(color).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var color = await _context.Colors.FindAsync(id);
        if (color != null)
        {
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
        }
    }
}
