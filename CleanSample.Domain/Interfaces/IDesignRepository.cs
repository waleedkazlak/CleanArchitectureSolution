namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IDesignRepository
{
    Task<Design?> GetByIdAsync(int id);
    Task<IEnumerable<Design>> GetAllAsync();
    Task<int> AddAsync(Design design);
    Task UpdateAsync(Design design);
    Task DeleteAsync(int id);
}
