using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IPickRequestLineRepository
{
    Task<PickRequestLine?> GetByIdAsync(long id);
    Task<IEnumerable<PickRequestLine>> GetAllAsync();
    Task<IEnumerable<PickRequestLine>> GetByPickRequestIdAsync(long pickRequestId);
    Task<long> AddAsync(PickRequestLine pickRequestLine);
    Task UpdateAsync(PickRequestLine pickRequestLine);
    Task DeleteAsync(long id);
}
