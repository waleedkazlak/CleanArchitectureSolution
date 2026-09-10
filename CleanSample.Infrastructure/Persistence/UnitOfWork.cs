using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace CleanSample.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly CleanSampleDbContext _context;
    private readonly IProductRepository _productRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IColorRepository _colorRepository;
    private readonly IMaterialRepository _materialRepository;
    private readonly IDesignRepository _designRepository;
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IPartRepository _partRepository;
    private readonly IProductBOMRepository _productBOMRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IClientLocationRepository _clientLocationRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderLineRepository _orderLineRepository;
    private readonly ILoadRequestRepository _loadRequestRepository;
    private readonly ILoadRequestLineRepository _loadRequestLineRepository;
    private readonly ILoadRequestPartRepository _loadRequestPartRepository;
    private readonly ILoadRepository _loadRepository;
    private readonly IVehicleOffloadRepository _vehicleOffloadRepository;
    private readonly IVehicleOffloadItemRepository _vehicleOffloadItemRepository;
    private readonly IFieldJobRepository _fieldJobRepository;
    private readonly IFieldAssemblyRepository _fieldAssemblyRepository;
    private readonly IIssueRepository _issueRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IScreenRepository _screenRepository;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(
        CleanSampleDbContext context,
        IProductRepository productRepository,
        IVehicleRepository vehicleRepository,
        ICategoryRepository categoryRepository,
        IColorRepository colorRepository,
        IMaterialRepository materialRepository,
        IDesignRepository designRepository,
        IProductVariantRepository productVariantRepository,
        IPartRepository partRepository,
        IProductBOMRepository productBOMRepository,
        IClientRepository clientRepository,
        IClientLocationRepository clientLocationRepository,
        IOrderRepository orderRepository,
        IOrderLineRepository orderLineRepository,
        ILoadRequestRepository loadRequestRepository,
        ILoadRequestLineRepository loadRequestLineRepository,
        ILoadRequestPartRepository loadRequestPartRepository,
        ILoadRepository loadRepository,
        IVehicleOffloadRepository vehicleOffloadRepository,
        IVehicleOffloadItemRepository vehicleOffloadItemRepository,
        IFieldJobRepository fieldJobRepository,
        IFieldAssemblyRepository fieldAssemblyRepository,
        IIssueRepository issueRepository,
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        IScreenRepository screenRepository,
        IRolePermissionRepository rolePermissionRepository,
        ILogger<UnitOfWork> logger)
    {
        _context = context;
        _productRepository = productRepository;
        _vehicleRepository = vehicleRepository;
        _categoryRepository = categoryRepository;
        _colorRepository = colorRepository;
        _materialRepository = materialRepository;
        _designRepository = designRepository;
        _productVariantRepository = productVariantRepository;
        _partRepository = partRepository;
        _productBOMRepository = productBOMRepository;
        _clientRepository = clientRepository;
        _clientLocationRepository = clientLocationRepository;
        _orderRepository = orderRepository;
        _orderLineRepository = orderLineRepository;
        _loadRequestRepository = loadRequestRepository;
        _loadRequestLineRepository = loadRequestLineRepository;
        _loadRequestPartRepository = loadRequestPartRepository;
        _loadRepository = loadRepository;
        _vehicleOffloadRepository = vehicleOffloadRepository;
        _vehicleOffloadItemRepository = vehicleOffloadItemRepository;
        _fieldJobRepository = fieldJobRepository;
        _fieldAssemblyRepository = fieldAssemblyRepository;
        _issueRepository = issueRepository;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _screenRepository = screenRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _logger = logger;
    }

    /// <summary>
    /// Gets the product repository instance
    /// </summary>
    public IProductRepository Products => _productRepository;

    /// <summary>
    /// Gets the vehicle repository instance
    /// </summary>
    public IVehicleRepository Vehicles => _vehicleRepository;

    /// <summary>
    /// Gets the category repository instance
    /// </summary>
    public ICategoryRepository Categories => _categoryRepository;

    /// <summary>
    /// Gets the color repository instance
    /// </summary>
    public IColorRepository Colors => _colorRepository;

    /// <summary>
    /// Gets the material repository instance
    /// </summary>
    public IMaterialRepository Materials => _materialRepository;

    /// <summary>
    /// Gets the design repository instance
    /// </summary>
    public IDesignRepository Designs => _designRepository;

    /// <summary>
    /// Gets the product variant repository instance
    /// </summary>
    public IProductVariantRepository ProductVariants => _productVariantRepository;

    /// <summary>
    /// Gets the part repository instance
    /// </summary>
    public IPartRepository Parts => _partRepository;

    /// <summary>
    /// Gets the product BOM repository instance
    /// </summary>
    public IProductBOMRepository ProductBOMs => _productBOMRepository;

    /// <summary>
    /// Gets the client repository instance
    /// </summary>
    public IClientRepository Clients => _clientRepository;

    /// <summary>
    /// Gets the client location repository instance
    /// </summary>
    public IClientLocationRepository ClientLocations => _clientLocationRepository;

    /// <summary>
    /// Gets the order repository instance
    /// </summary>
    public IOrderRepository Orders => _orderRepository;

    /// <summary>
    /// Gets the order line repository instance
    /// </summary>
    public IOrderLineRepository OrderLines => _orderLineRepository;

    /// <summary>
    /// Gets the load request repository instance
    /// </summary>
    public ILoadRequestRepository LoadRequests => _loadRequestRepository;

    /// <summary>
    /// Gets the load request line repository instance
    /// </summary>
    public ILoadRequestLineRepository LoadRequestLines => _loadRequestLineRepository;

    /// <summary>
    /// Gets the load request part repository instance
    /// </summary>
    public ILoadRequestPartRepository LoadRequestParts => _loadRequestPartRepository;

    /// <summary>
    /// Gets the load repository instance
    /// </summary>
    public ILoadRepository Loads => _loadRepository;

    /// <summary>
    /// Gets the vehicle offload repository instance
    /// </summary>
    public IVehicleOffloadRepository VehicleOffloads => _vehicleOffloadRepository;

    /// <summary>
    /// Gets the vehicle offload item repository instance
    /// </summary>
    public IVehicleOffloadItemRepository VehicleOffloadItems => _vehicleOffloadItemRepository;

    /// <summary>
    /// Gets the field job repository instance
    /// </summary>
    public IFieldJobRepository FieldJobs => _fieldJobRepository;

    /// <summary>
    /// Gets the field assembly repository instance
    /// </summary>
    public IFieldAssemblyRepository FieldAssemblies => _fieldAssemblyRepository;

    /// <summary>
    /// Gets the issue repository instance
    /// </summary>
    public IIssueRepository Issues => _issueRepository;

    /// <summary>
    /// Gets the role repository instance
    /// </summary>
    public IRoleRepository Roles => _roleRepository;

    /// <summary>
    /// Gets the user repository instance
    /// </summary>
    public IUserRepository Users => _userRepository;

    /// <summary>
    /// Gets the screen repository instance
    /// </summary>
    public IScreenRepository Screens => _screenRepository;

    /// <summary>
    /// Gets the role permission repository instance
    /// </summary>
    public IRolePermissionRepository RolePermissions => _rolePermissionRepository;

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
