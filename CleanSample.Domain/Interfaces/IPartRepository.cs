namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IPartRepository
{
    Task<Part?> GetByIdAsync(int id);
    Task<IEnumerable<Part>> GetAllAsync();
    Task<int> AddAsync(Part part);
    Task UpdateAsync(Part part);
    Task DeleteAsync(int id);
}
