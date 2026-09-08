namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IProductVariantRepository
{
    Task<ProductVariant?> GetByIdAsync(int id);
    Task<IEnumerable<ProductVariant>> GetAllAsync();
    Task<int> AddAsync(ProductVariant productVariant);
    Task UpdateAsync(ProductVariant productVariant);
    Task DeleteAsync(int id);
}
