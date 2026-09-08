namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IMaterialRepository
{
    Task<Material?> GetByIdAsync(int id);
    Task<IEnumerable<Material>> GetAllAsync();
    Task<int> AddAsync(Material material);
    Task UpdateAsync(Material material);
    Task DeleteAsync(int id);
}
