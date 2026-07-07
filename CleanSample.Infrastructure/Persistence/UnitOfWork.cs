

using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace CleanSample.Infrastructure.Persistence;
public class UnitOfWork : IUnitOfWork
{
    private readonly CleanSampleDbContext _context;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(
        CleanSampleDbContext context,
        IProductRepository productRepository,
        ILogger<UnitOfWork> logger)
    {
        _context = context;
        _productRepository = productRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gets the product repository instance
    /// </summary>
    public IProductRepository Products => _productRepository;

    /// <summary>
    /// Saves all changes made to the database asynchronously
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        try
        {
            _logger.LogInformation("Saving changes to database");
            var result = await _context.SaveChangesAsync();
            _logger.LogInformation("Successfully saved {Count} changes to database", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving changes to database");
            throw;
        }
    }

    /// <summary>
    /// Saves all changes made to the database asynchronously with cancellation support
    /// </summary>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Saving changes to database with cancellation token");
            var result = await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Successfully saved {Count} changes to database", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while saving changes to database");
            throw;
        }
    }

    /// <summary>
    /// Begins a new database transaction
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        try
        {
            _logger.LogInformation("Beginning database transaction");
            _transaction = await _context.Database.BeginTransactionAsync();
            _logger.LogInformation("Database transaction started successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while beginning database transaction");
            throw;
        }
    }

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        try
        {
            if (_transaction == null)
            {
                _logger.LogWarning("No active transaction to commit");
                return;
            }

            _logger.LogInformation("Committing database transaction");
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
            _logger.LogInformation("Database transaction committed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while committing database transaction");
            throw;
        }
    }

    /// <summary>
    /// Rollbacks the current transaction
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        try
        {
            if (_transaction == null)
            {
                _logger.LogWarning("No active transaction to rollback");
                return;
            }

            _logger.LogInformation("Rolling back database transaction");
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
            _logger.LogInformation("Database transaction rolled back successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while rolling back database transaction");
            throw;
        }
    }

    /// <summary>
    /// Disposes the DbContext and releases resources
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
        _logger.LogInformation("UnitOfWork disposed");
    }
}
