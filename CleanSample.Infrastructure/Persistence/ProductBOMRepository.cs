using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class ProductBOMRepository : IProductBOMRepository
{
    private readonly CleanSampleDbContext _context;

    public ProductBOMRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<ProductBOM?> GetByIdAsync(int id)
    {
        return await _context.ProductBOMs
            .Include(pb => pb.ProductVariant)
            .Include(pb => pb.Part)
            .FirstOrDefaultAsync(pb => pb.Id == id);
    }

    public async Task<IEnumerable<ProductBOM>> GetAllAsync()
    {
        return await _context.ProductBOMs
            .Include(pb => pb.ProductVariant)
            .Include(pb => pb.Part)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductBOM>> GetByProductVariantIdAsync(int productVariantId)
    {
        return await _context.ProductBOMs
            .Include(pb => pb.ProductVariant)
            .Include(pb => pb.Part)
            .Where(pb => pb.ProductVariantId == productVariantId)
            .ToListAsync();
    }

    public async Task<ProductBOM?> GetByVariantAndPartIdAsync(int productVariantId, int partId)
    {
        return await _context.ProductBOMs
            .Include(pb => pb.ProductVariant)
            .Include(pb => pb.Part)
            .FirstOrDefaultAsync(pb => pb.ProductVariantId == productVariantId && pb.PartId == partId);
    }

    public async Task<int> AddAsync(ProductBOM productBom)
    {
        _context.ProductBOMs.Add(productBom);
        await _context.SaveChangesAsync();
        return productBom.Id;
    }

    public async Task<List<ProductBOM>> AddRangeAsync(IEnumerable<ProductBOM> productBoms)
    {
        var bomList = productBoms.ToList();
        _context.ProductBOMs.AddRange(bomList);
        await _context.SaveChangesAsync();
        return bomList;
    }

    public async Task UpdateAsync(ProductBOM productBom)
    {
        _context.Entry(productBom).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<ProductBOM>> UpdateRangeAsync(IEnumerable<ProductBOM> productBoms)
    {
        var bomList = productBoms.ToList();
        foreach (var bom in bomList)
        {
            _context.Entry(bom).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
        return bomList;
    }

    public async Task DeleteAsync(int id)
    {
        var productBom = await _context.ProductBOMs.FindAsync(id);
        if (productBom != null)
        {
            _context.ProductBOMs.Remove(productBom);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> DeleteRangeAsync(IEnumerable<int> ids)
    {
        var idList = ids.ToList();
        var boms = await _context.ProductBOMs.Where(pb => idList.Contains(pb.Id)).ToListAsync();
        if (boms.Any())
        {
            _context.ProductBOMs.RemoveRange(boms);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
