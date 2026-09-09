using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IFieldJobRepository
{
    Task<FieldJob?> GetByIdAsync(long id);
    Task<FieldJob?> GetByJobNumberAsync(string jobNumber);
    Task<IEnumerable<FieldJob>> GetAllAsync();
    Task<IEnumerable<FieldJob>> GetByPickRequestIdAsync(long pickRequestId);
    Task<IEnumerable<FieldJob>> GetByClientIdAsync(int clientId);
    Task<IEnumerable<FieldJob>> GetByTechnicianIdAsync(int technicianId);
    Task<IEnumerable<FieldJob>> GetBySupervisorIdAsync(int supervisorId);
    Task<long> AddAsync(FieldJob fieldJob);
    Task UpdateAsync(FieldJob fieldJob);
    Task DeleteAsync(long id);
}
