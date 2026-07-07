namespace CleanSample.Domain.Interfaces;

/// <summary>
/// Unit of Work interface for coordinating repositories and database operations
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Product repository instance
    /// </summary>
    IProductRepository Products { get; }

    /// <summary>
    /// Saves all changes made to the database asynchronously
    /// </summary>
    /// <returns>Number of state entries written to the database</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Saves all changes made to the database asynchronously with cancellation support
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of state entries written to the database</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Begins a new database transaction
    /// </summary>
    /// <returns>Database transaction instance</returns>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rollbacks the current transaction
    /// </summary>
    Task RollbackTransactionAsync();
}
