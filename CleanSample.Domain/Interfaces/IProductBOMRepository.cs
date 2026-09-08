namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IProductBOMRepository
{
    Task<ProductBOM?> GetByIdAsync(int id);
    Task<IEnumerable<ProductBOM>> GetAllAsync();
    Task<IEnumerable<ProductBOM>> GetByProductVariantIdAsync(int productVariantId);
    Task<int> AddAsync(ProductBOM productBom);
    Task UpdateAsync(ProductBOM productBom);
    Task DeleteAsync(int id);
}
