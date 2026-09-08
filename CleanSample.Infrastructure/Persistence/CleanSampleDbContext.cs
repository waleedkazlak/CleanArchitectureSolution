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

    /// <summary>
    /// Materials DbSet
    /// </summary>
    public DbSet<Material> Materials { get; set; }

    /// <summary>
    /// Designs DbSet
    /// </summary>
    public DbSet<Design> Designs { get; set; }

    /// <summary>
    /// ProductVariants DbSet
    /// </summary>
    public DbSet<ProductVariant> ProductVariants { get; set; }

    /// <summary>
    /// Parts DbSet
    /// </summary>
    public DbSet<Part> Parts { get; set; }

    /// <summary>
    /// ProductBOMs DbSet
    /// </summary>
    public DbSet<ProductBOM> ProductBOMs { get; set; }

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

        // Configure Material entity
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("MaterialId");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Materials_Name");

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("Materials");
        });

        // Configure Design entity
        modelBuilder.Entity<Design>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("DesignId");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Designs_Name");

            entity.Property(e => e.Code)
                .HasMaxLength(50);

            entity.HasIndex(e => e.Code)
                .IsUnique()
                .HasDatabaseName("UQ_Designs_Code");

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("Designs");
        });

        // Configure ProductVariant entity
        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("ProductVariantId");

            entity.Property(e => e.ProductId)
                .IsRequired();

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .HasConstraintName("FK_ProductVariants_Products")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Color)
                .WithMany()
                .HasForeignKey(e => e.ColorId)
                .HasConstraintName("FK_ProductVariants_Colors")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Material)
                .WithMany()
                .HasForeignKey(e => e.MaterialId)
                .HasConstraintName("FK_ProductVariants_Materials")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Design)
                .WithMany()
                .HasForeignKey(e => e.DesignId)
                .HasConstraintName("FK_ProductVariants_Designs")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(e => e.Code)
                .IsUnique()
                .HasDatabaseName("UQ_ProductVariants_Code");

            entity.Property(e => e.Barcode)
                .HasMaxLength(100);

            entity.HasIndex(e => e.Barcode)
                .IsUnique()
                .HasDatabaseName("UQ_ProductVariants_Barcode");

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("ProductVariants");
        });

        // Configure Part entity
        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("PartId");

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(e => e.Code)
                .IsUnique()
                .HasDatabaseName("UQ_Parts_Code");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.Barcode)
                .HasMaxLength(100);

            entity.HasIndex(e => e.Barcode)
                .IsUnique()
                .HasDatabaseName("UQ_Parts_Barcode");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("Parts");
        });

        // Configure ProductBOM entity
        modelBuilder.Entity<ProductBOM>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("ProductBOMId");

            entity.Property(e => e.ProductVariantId)
                .IsRequired();

            entity.Property(e => e.PartId)
                .IsRequired();

            entity.HasOne(e => e.ProductVariant)
                .WithMany()
                .HasForeignKey(e => e.ProductVariantId)
                .HasConstraintName("FK_ProductBOM_ProductVariants")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Part)
                .WithMany()
                .HasForeignKey(e => e.PartId)
                .HasConstraintName("FK_ProductBOM_Parts")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.ProductVariantId, e.PartId })
                .IsUnique()
                .HasDatabaseName("UQ_ProductBOM_Variant_Part");

            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .IsRequired();

            entity.ToTable("ProductBOM", t => t.HasCheckConstraint("CK_ProductBOM_Quantity", "[Quantity] > 0"));

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });
    }
}