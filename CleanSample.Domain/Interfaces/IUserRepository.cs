namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUserNameAsync(string userName);
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllAsync();
    Task<IEnumerable<User>> GetByRoleIdAsync(int roleId);
    Task<int> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
}
