namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IClientLocationRepository
{
    Task<ClientLocation?> GetByIdAsync(int id);
    Task<IEnumerable<ClientLocation>> GetAllAsync();
    Task<IEnumerable<ClientLocation>> GetByClientIdAsync(int clientId);
    Task<ClientLocation?> GetByNameAndClientIdAsync(int clientId, string name);
    Task<int> AddAsync(ClientLocation clientLocation);
    Task<List<ClientLocation>> AddRangeAsync(IEnumerable<ClientLocation> clientLocations);
    Task UpdateAsync(ClientLocation clientLocation);
    Task<List<ClientLocation>> UpdateRangeAsync(IEnumerable<ClientLocation> clientLocations);
    Task DeleteAsync(int id);
    Task<bool> DeleteRangeAsync(IEnumerable<int> ids);
}
