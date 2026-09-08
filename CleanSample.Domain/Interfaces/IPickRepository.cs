using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IPickRepository
{
    Task<Pick?> GetByIdAsync(long id);
    Task<IEnumerable<Pick>> GetAllAsync();
    Task<IEnumerable<Pick>> GetByPickRequestIdAsync(long pickRequestId);
    Task<IEnumerable<Pick>> GetByPickRequestPartIdAsync(long pickRequestPartId);
    Task<IEnumerable<Pick>> GetByDriverIdAsync(int driverId);
    Task<IEnumerable<Pick>> GetByPickedByAsync(int pickedBy);
    Task<long> AddAsync(Pick pick);
    Task<IEnumerable<long>> AddRangeAsync(IEnumerable<Pick> picks);
    Task UpdateAsync(Pick pick);
    Task UpdateRangeAsync(IEnumerable<Pick> picks);
    Task DeleteAsync(long id);
    Task DeleteRangeAsync(IEnumerable<long> ids);
}
