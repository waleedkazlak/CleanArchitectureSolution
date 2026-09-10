using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface ILoadRequestLineRepository
{
    Task<LoadRequestLine?> GetByIdAsync(long id);
    Task<IEnumerable<LoadRequestLine>> GetAllAsync();
    Task<IEnumerable<LoadRequestLine>> GetByLoadRequestIdAsync(long loadRequestId);
    Task<long> AddAsync(LoadRequestLine loadRequestLine);
    Task UpdateAsync(LoadRequestLine loadRequestLine);
    Task DeleteAsync(long id);
}
