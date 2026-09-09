namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(int id);
    Task<Role?> GetByNameAsync(string name);
    Task<IEnumerable<Role>> GetAllAsync();
    Task<int> AddAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(int id);
}
