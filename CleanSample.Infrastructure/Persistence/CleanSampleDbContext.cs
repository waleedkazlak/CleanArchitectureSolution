using CleanSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

/// <summary>
/// Database context for CleanSample application
/// </summary>
public class CleanSampleDbContext : DbContext
{
    public CleanSampleDbContext(DbContextOptions<CleanSampleDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Products DbSet
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Users DbSet
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Vehicles DbSet
    /// </summary>
    public DbSet<Vehicle> Vehicles { get; set; }

    /// <summary>
    /// Categories DbSet
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    /// Colors DbSet
    /// </summary>
    public DbSet<Color> Colors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.Price)
                .HasPrecision(18, 2);

            entity.Property(e => e.CategoryId)
                .IsRequired();

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Add table name
            entity.ToTable("Products");
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.Username)
                .IsUnique();

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("User");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Add table name
            entity.ToTable("Users");
        });

        // Configure Vehicle entity
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("VehicleId");

            entity.Property(e => e.VehicleNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.VehicleNumber)
                .IsUnique()
                .HasDatabaseName("UQ_Vehicles_VehicleNumber");

            entity.Property(e => e.PlateNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.PlateNumber)
                .IsUnique()
                .HasDatabaseName("UQ_Vehicles_PlateNumber");

            entity.Property(e => e.VehicleType)
                .HasMaxLength(100);

            entity.Property(e => e.CapacityKg)
                .HasPrecision(18, 2);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("Vehicles");
        });

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("CategoryId");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Categories_Name");

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("Categories");
        });

        // Configure Color entity
        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("ColorId");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Colors_Name");

            entity.Property(e => e.Code)
                .HasMaxLength(50);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("Colors");
        });
    }
}