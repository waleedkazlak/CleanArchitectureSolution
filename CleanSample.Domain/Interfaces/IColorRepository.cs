namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IColorRepository
{
    Task<Color?> GetByIdAsync(int id);
    Task<IEnumerable<Color>> GetAllAsync();
    Task<int> AddAsync(Color color);
    Task UpdateAsync(Color color);
    Task DeleteAsync(int id);
}
