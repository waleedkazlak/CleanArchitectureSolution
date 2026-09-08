namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IClientLocationRepository
{
    Task<ClientLocation?> GetByIdAsync(int id);
    Task<IEnumerable<ClientLocation>> GetAllAsync();
    Task<IEnumerable<ClientLocation>> GetByClientIdAsync(int clientId);
    Task<int> AddAsync(ClientLocation clientLocation);
    Task UpdateAsync(ClientLocation clientLocation);
    Task DeleteAsync(int id);
}
