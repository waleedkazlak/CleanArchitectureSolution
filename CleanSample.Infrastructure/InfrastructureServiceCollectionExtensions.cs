using CleanSample.Domain.Interfaces;
using CleanSample.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSample.Infrastructure;

/// <summary>
/// Dependency injection extension methods for infrastructure layer
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Register DbContext with SQL Server
        services.AddDbContext<CleanSampleDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            }));

        // Register repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IColorRepository, ColorRepository>();
        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IDesignRepository, DesignRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<IPartRepository, PartRepository>();
        services.AddScoped<IProductBOMRepository, ProductBOMRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IClientLocationRepository, ClientLocationRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderLineRepository, OrderLineRepository>();
        services.AddScoped<IPickRequestRepository, PickRequestRepository>();
        services.AddScoped<IPickRequestLineRepository, PickRequestLineRepository>();
        services.AddScoped<IPickRequestPartRepository, PickRequestPartRepository>();
        services.AddScoped<IPickRepository, PickRepository>();
        services.AddScoped<IVehicleLoadRepository, VehicleLoadRepository>();
        services.AddScoped<IVehicleLoadItemRepository, VehicleLoadItemRepository>();
        services.AddScoped<IFieldJobRepository, FieldJobRepository>();
        services.AddScoped<IFieldAssemblyRepository, FieldAssemblyRepository>();
        services.AddScoped<IIssueRepository, IssueRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}