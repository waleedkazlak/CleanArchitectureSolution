using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IScreenRepository
{
    Task<Screen?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Screen?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<Screen?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Screen>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Screen>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<Screen> AddAsync(Screen screen, CancellationToken cancellationToken = default);
    Task UpdateAsync(Screen screen, CancellationToken cancellationToken = default);
    Task DeleteAsync(Screen screen, CancellationToken cancellationToken = default);
}
