using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IFieldAssemblyRepository
{
    Task<FieldAssembly?> GetByIdAsync(long id);
    Task<IEnumerable<FieldAssembly>> GetAllAsync();
    Task<IEnumerable<FieldAssembly>> GetByFieldJobIdAsync(long fieldJobId);
    Task<IEnumerable<FieldAssembly>> GetByProductIdAsync(int productId);
    Task<IEnumerable<FieldAssembly>> GetByTechnicianIdAsync(int technicianId);
    Task<IEnumerable<FieldAssembly>> GetBySupervisorIdAsync(int supervisorId);
    Task<long> AddAsync(FieldAssembly fieldAssembly);
    Task UpdateAsync(FieldAssembly fieldAssembly);
    Task DeleteAsync(long id);
}
