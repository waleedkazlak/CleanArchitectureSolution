using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface ILoadRequestPartRepository
{
    Task<LoadRequestPart?> GetByIdAsync(long id);
    Task<IEnumerable<LoadRequestPart>> GetAllAsync();
    Task<IEnumerable<LoadRequestPart>> GetByLoadRequestIdAsync(long loadRequestId);
    Task<IEnumerable<LoadRequestPart>> GetByLoadRequestLineIdAsync(long loadRequestLineId);
    Task<long> AddAsync(LoadRequestPart loadRequestPart);
    Task UpdateAsync(LoadRequestPart loadRequestPart);
    Task DeleteAsync(long id);
}
