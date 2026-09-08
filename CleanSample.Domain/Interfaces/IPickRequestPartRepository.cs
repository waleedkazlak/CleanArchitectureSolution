using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IPickRequestPartRepository
{
    Task<PickRequestPart?> GetByIdAsync(long id);
    Task<IEnumerable<PickRequestPart>> GetAllAsync();
    Task<IEnumerable<PickRequestPart>> GetByPickRequestIdAsync(long pickRequestId);
    Task<IEnumerable<PickRequestPart>> GetByPickRequestLineIdAsync(long pickRequestLineId);
    Task<long> AddAsync(PickRequestPart pickRequestPart);
    Task UpdateAsync(PickRequestPart pickRequestPart);
    Task DeleteAsync(long id);
}
