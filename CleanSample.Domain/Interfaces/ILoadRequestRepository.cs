using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface ILoadRequestRepository
{
    IQueryable<LoadRequest> GetQueryable(bool includeFieldJobs = false);
    Task<LoadRequest?> GetByIdAsync(long id);
    Task<IEnumerable<LoadRequest>> GetAllAsync();
    Task<IEnumerable<LoadRequest>> GetByOrderIdAsync(long orderId);
    Task<IEnumerable<LoadRequest>> GetByClientIdAsync(int clientId);
    Task<long> AddAsync(LoadRequest loadRequest);
    Task UpdateAsync(LoadRequest loadRequest);
    Task DeleteAsync(long id);
}
