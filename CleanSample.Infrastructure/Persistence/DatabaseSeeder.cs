using CleanSample.Application.Services;
using CleanSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

/// <summary>
/// Database seeder for initial data setup
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds the database with initial data
    /// </summary>
    public static async Task SeedAsync(CleanSampleDbContext context)
    {
        // Check if the database already has data
        if (await context.Products.AnyAsync() || await context.Users.AnyAsync())
        {
            return; // Database is already seeded
        }

        try
        {
            // Seed Users first (as Products might depend on them)
            await SeedUsersAsync(context);

            // Seed Products
            await SeedProductsAsync(context);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error occurred while seeding the database", ex);
        }
    }

    /// <summary>
    /// Seeds user data
    /// </summary>
    private static async Task SeedUsersAsync(CleanSampleDbContext context)
    {
        var authService = new AuthenticationService(
            new Application.DTOs.JwtSettingsDto
            {
                SecretKey = "E1CC9E8B-D819-49D8-B1C6-8CA9199F2132",
                Issuer = "CleanSample",
                Audience = "CleanSampleUsers",
                ExpirationMinutes = 60
            },
            null!); // Logger is not needed for seeding

        var users = new List<User>
        {
            new User
            {
                Username = "admin",
                Email = "admin@example.com",
                FullName = "Administrator",
                PasswordHash = authService.HashPassword("admin123"),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = null
            },
            new User
            {
                Username = "user",
                Email = "user@example.com",
                FullName = "Regular User",
                PasswordHash = authService.HashPassword("user123"),
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = null
            },
            new User
            {
                Username = "manager",
                Email = "manager@example.com",
                FullName = "Manager User",
                PasswordHash = authService.HashPassword("manager123"),
                Role = "Manager",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = null
            },
            new User
            {
                Username = "testuser",
                Email = "testuser@example.com",
                FullName = "Test User",
                PasswordHash = authService.HashPassword("test123"),
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                LastLogin = null
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds product data
    /// </summary>
    private static async Task SeedProductsAsync(CleanSampleDbContext context)
    {
        var products = new List<Product>
        {
            new Product
            {
                Name = "Laptop",
                Description = "High-performance laptop for professionals with Intel i7 processor and 16GB RAM",
                Price = 999.99m,
                Stock = 50,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Smartphone",
                Description = "Latest generation smartphone with advanced features, 5G connectivity, and excellent camera",
                Price = 699.99m,
                Stock = 100,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Tablet",
                Description = "Portable tablet for entertainment and productivity with 10-inch display",
                Price = 399.99m,
                Stock = 75,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Monitor",
                Description = "4K display monitor for professional use with color accuracy and HDR support",
                Price = 549.99m,
                Stock = 30,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Keyboard",
                Description = "Mechanical keyboard with RGB lighting and programmable keys for gaming and typing",
                Price = 129.99m,
                Stock = 200,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Mouse",
                Description = "Wireless mouse with ergonomic design and precision tracking",
                Price = 49.99m,
                Stock = 150,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Headphones",
                Description = "Noise-cancelling headphones with high-quality audio and 30-hour battery life",
                Price = 299.99m,
                Stock = 80,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Webcam",
                Description = "1080p HD webcam with auto-focus and built-in microphone for video conferencing",
                Price = 79.99m,
                Stock = 120,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "USB Hub",
                Description = "7-port USB 3.0 hub with fast charging support",
                Price = 39.99m,
                Stock = 250,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "External SSD",
                Description = "1TB external solid state drive with fast transfer speeds and durability",
                Price = 149.99m,
                Stock = 100,
                IsActive = true,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}