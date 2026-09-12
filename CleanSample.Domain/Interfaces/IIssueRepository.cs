using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IIssueRepository
{
    IQueryable<Issue> GetQueryable();
    Task<Issue?> GetByIdAsync(long id);
    Task<IEnumerable<Issue>> GetAllAsync();
    Task<IEnumerable<Issue>> GetByLoadRequestIdAsync(long loadRequestId);
    Task<IEnumerable<Issue>> GetByFieldJobIdAsync(long fieldJobId);
    Task<IEnumerable<Issue>> GetByFieldAssemblyIdAsync(long fieldAssemblyId);
    Task<IEnumerable<Issue>> GetByReportedByAsync(int reportedBy);
    Task<IEnumerable<Issue>> GetByResolvedByAsync(int resolvedBy);
    Task<long> AddAsync(Issue issue);
    Task UpdateAsync(Issue issue);
    Task DeleteAsync(long id);
}
