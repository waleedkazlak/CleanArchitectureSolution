using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class ProductVariantRepository : IProductVariantRepository
{
    private readonly CleanSampleDbContext _context;

    public ProductVariantRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<ProductVariant?> GetByIdAsync(int id)
    {
        return await _context.ProductVariants
            .Include(pv => pv.Product)
            .Include(pv => pv.Color)
            .Include(pv => pv.Material)
            .Include(pv => pv.Design)
            .FirstOrDefaultAsync(pv => pv.Id == id);
    }

    public async Task<IEnumerable<ProductVariant>> GetAllAsync()
    {
        return await _context.ProductVariants
            .Include(pv => pv.Product)
            .Include(pv => pv.Color)
            .Include(pv => pv.Material)
            .Include(pv => pv.Design)
            .ToListAsync();
    }

    public async Task<int> AddAsync(ProductVariant productVariant)
    {
        _context.ProductVariants.Add(productVariant);
        await _context.SaveChangesAsync();
        return productVariant.Id;
    }

    public async Task UpdateAsync(ProductVariant productVariant)
    {
        _context.Entry(productVariant).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var productVariant = await _context.ProductVariants.FindAsync(id);
        if (productVariant != null)
        {
            _context.ProductVariants.Remove(productVariant);
            await _context.SaveChangesAsync();
        }
    }
}
