using CleanSample.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSample.Application;

/// <summary>
/// Dependency injection extension methods for application layer
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Get the assembly containing this type
        var assembly = typeof(ApplicationServiceCollectionExtensions).Assembly;

        // Register MediatR with handlers from this assembly
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        // Register Fluent Validators from this assembly
        services.AddValidatorsFromAssembly(assembly);

        // Register Memory Cache, Permission Checker Service, and Load Request BOM Service
        services.AddMemoryCache();
        services.AddScoped<Services.IPermissionCheckerService, Services.PermissionCheckerService>();
        services.AddScoped<Services.ILoadRequestBOMService, Services.LoadRequestBOMService>();

        return services;
    }
}