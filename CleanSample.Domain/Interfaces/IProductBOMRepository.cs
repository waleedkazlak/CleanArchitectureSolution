namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IProductBOMRepository
{
    Task<ProductBOM?> GetByIdAsync(int id);
    Task<IEnumerable<ProductBOM>> GetAllAsync();
    Task<IEnumerable<ProductBOM>> GetByProductVariantIdAsync(int productVariantId);
    Task<ProductBOM?> GetByVariantAndPartIdAsync(int productVariantId, int partId);
    Task<int> AddAsync(ProductBOM productBom);
    Task<List<ProductBOM>> AddRangeAsync(IEnumerable<ProductBOM> productBoms);
    Task UpdateAsync(ProductBOM productBom);
    Task<List<ProductBOM>> UpdateRangeAsync(IEnumerable<ProductBOM> productBoms);
    Task DeleteAsync(int id);
    Task<bool> DeleteRangeAsync(IEnumerable<int> ids);
}
