using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface ILoadRequestLineRepository
{
    IQueryable<LoadRequestLine> GetQueryable();
    Task<LoadRequestLine?> GetByIdAsync(long id);
    Task<IEnumerable<LoadRequestLine>> GetAllAsync();
    Task<IEnumerable<LoadRequestLine>> GetByLoadRequestIdAsync(long loadRequestId);
    Task<LoadRequestLine?> GetByLoadRequestAndProductIdAsync(long loadRequestId, int productId);
    Task<long> AddAsync(LoadRequestLine loadRequestLine);
    Task<List<LoadRequestLine>> AddRangeAsync(IEnumerable<LoadRequestLine> loadRequestLines);
    Task UpdateAsync(LoadRequestLine loadRequestLine);
    Task<List<LoadRequestLine>> UpdateRangeAsync(IEnumerable<LoadRequestLine> loadRequestLines);
    Task DeleteAsync(long id);
    Task<bool> DeleteRangeAsync(IEnumerable<long> ids);
}
