namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(int id);
    Task<IEnumerable<Client>> GetAllAsync();
    Task<int> AddAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(int id);
}
