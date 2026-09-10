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
    /// Vehicle repository instance
    /// </summary>
    IVehicleRepository Vehicles { get; }

    /// <summary>
    /// Category repository instance
    /// </summary>
    ICategoryRepository Categories { get; }

    /// <summary>
    /// Color repository instance
    /// </summary>
    IColorRepository Colors { get; }

    /// <summary>
    /// Material repository instance
    /// </summary>
    IMaterialRepository Materials { get; }

    /// <summary>
    /// Design repository instance
    /// </summary>
    IDesignRepository Designs { get; }

    /// <summary>
    /// ProductVariant repository instance
    /// </summary>
    IProductVariantRepository ProductVariants { get; }

    /// <summary>
    /// Part repository instance
    /// </summary>
    IPartRepository Parts { get; }

    /// <summary>
    /// ProductBOM repository instance
    /// </summary>
    IProductBOMRepository ProductBOMs { get; }

    /// <summary>
    /// Client repository instance
    /// </summary>
    IClientRepository Clients { get; }

    /// <summary>
    /// ClientLocation repository instance
    /// </summary>
    IClientLocationRepository ClientLocations { get; }

    /// <summary>
    /// Order repository instance
    /// </summary>
    IOrderRepository Orders { get; }

    /// <summary>
    /// OrderLine repository instance
    /// </summary>
    IOrderLineRepository OrderLines { get; }

    /// <summary>
    /// LoadRequest repository instance
    /// </summary>
    ILoadRequestRepository LoadRequests { get; }

    /// <summary>
    /// LoadRequestLine repository instance
    /// </summary>
    ILoadRequestLineRepository LoadRequestLines { get; }

    /// <summary>
    /// LoadRequestPart repository instance
    /// </summary>
    ILoadRequestPartRepository LoadRequestParts { get; }

    /// <summary>
    /// Load repository instance
    /// </summary>
    ILoadRepository Loads { get; }

    /// <summary>
    /// VehicleOffload repository instance
    /// </summary>
    IVehicleOffloadRepository VehicleOffloads { get; }

    /// <summary>
    /// VehicleOffloadItem repository instance
    /// </summary>
    IVehicleOffloadItemRepository VehicleOffloadItems { get; }

    /// <summary>
    /// FieldJob repository instance
    /// </summary>
    IFieldJobRepository FieldJobs { get; }

    /// <summary>
    /// FieldAssembly repository instance
    /// </summary>
    IFieldAssemblyRepository FieldAssemblies { get; }

    /// <summary>
    /// Issue repository instance
    /// </summary>
    IIssueRepository Issues { get; }

    /// <summary>
    /// Role repository instance
    /// </summary>
    IRoleRepository Roles { get; }

    /// <summary>
    /// User repository instance
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Screen repository instance
    /// </summary>
    IScreenRepository Screens { get; }

    /// <summary>
    /// RolePermission repository instance
    /// </summary>
    IRolePermissionRepository RolePermissions { get; }


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
