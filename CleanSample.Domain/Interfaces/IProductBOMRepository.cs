namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IProductBOMRepository
{
    Task<ProductBOM?> GetByIdAsync(int id);
    Task<IEnumerable<ProductBOM>> GetAllAsync();
    Task<IEnumerable<ProductBOM>> GetByProductIdAsync(int productId);
    Task<ProductBOM?> GetByProductAndPartIdAsync(int productId, int partId);
    Task<int> AddAsync(ProductBOM productBom);
    Task<List<ProductBOM>> AddRangeAsync(IEnumerable<ProductBOM> productBoms);
    Task UpdateAsync(ProductBOM productBom);
    Task<List<ProductBOM>> UpdateRangeAsync(IEnumerable<ProductBOM> productBoms);
    Task DeleteAsync(int id);
    Task<bool> DeleteRangeAsync(IEnumerable<int> ids);
}
