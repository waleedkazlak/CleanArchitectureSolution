namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

/// <summary>
/// Interface for product repository following the Repository pattern
/// </summary>
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<int> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}
