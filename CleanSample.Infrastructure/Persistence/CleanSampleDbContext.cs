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

    /// <summary>
    /// Clients DbSet
    /// </summary>
    public DbSet<Client> Clients { get; set; }

    /// <summary>
    /// ClientLocations DbSet
    /// </summary>
    public DbSet<ClientLocation> ClientLocations { get; set; }

    /// <summary>
    /// Orders DbSet
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// OrderLines DbSet
    /// </summary>
    public DbSet<OrderLine> OrderLines { get; set; }

    /// <summary>
    /// PickRequests DbSet
    /// </summary>
    public DbSet<PickRequest> PickRequests { get; set; }

    /// <summary>
    /// PickRequestLines DbSet
    /// </summary>
    public DbSet<PickRequestLine> PickRequestLines { get; set; }

    /// <summary>
    /// PickRequestParts DbSet
    /// </summary>
    public DbSet<PickRequestPart> PickRequestParts { get; set; }

    /// <summary>
    /// Picks DbSet
    /// </summary>
    public DbSet<Pick> Picks { get; set; }

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

        // Configure Client entity
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("ClientId");

            entity.Property(e => e.Code)
                .HasMaxLength(50);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(e => e.Phone)
                .HasMaxLength(50);

            entity.Property(e => e.Mobile)
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .HasMaxLength(250);

            entity.Property(e => e.Address)
                .HasMaxLength(500);

            entity.Property(e => e.City)
                .HasMaxLength(150);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("Clients");
        });

        // Configure ClientLocation entity
        modelBuilder.Entity<ClientLocation>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("ClientLocationId");

            entity.Property(e => e.ClientId)
                .IsRequired();

            entity.HasOne(e => e.Client)
                .WithMany()
                .HasForeignKey(e => e.ClientId)
                .HasConstraintName("FK_ClientLocations_Clients")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.City)
                .HasMaxLength(150);

            entity.Property(e => e.ContactName)
                .HasMaxLength(200);

            entity.Property(e => e.ContactPhone)
                .HasMaxLength(50);

            entity.Property(e => e.Latitude)
                .HasPrecision(10, 7);

            entity.Property(e => e.Longitude)
                .HasPrecision(10, 7);

            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("ClientLocations");
        });

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("OrderId");

            entity.Property(e => e.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.OrderNumber)
                .IsUnique()
                .HasDatabaseName("UQ_Orders_OrderNumber");

            entity.Property(e => e.ClientId)
                .IsRequired();

            entity.HasOne(e => e.Client)
                .WithMany()
                .HasForeignKey(e => e.ClientId)
                .HasConstraintName("FK_Orders_Clients")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(e => e.RequiredDate)
                .HasColumnType("date");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Draft");

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("Orders");
        });

        // Configure OrderLine entity
        modelBuilder.Entity<OrderLine>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("OrderLineId");

            entity.Property(e => e.OrderId)
                .IsRequired();

            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(e => e.OrderId)
                .HasConstraintName("FK_OrderLines_Orders")
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.ProductVariantId)
                .IsRequired();

            entity.HasOne(e => e.ProductVariant)
                .WithMany()
                .HasForeignKey(e => e.ProductVariantId)
                .HasConstraintName("FK_OrderLines_ProductVariants")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.ToTable("OrderLines", t => t.HasCheckConstraint("CK_OrderLines_Quantity", "[Quantity] > 0"));

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // Configure PickRequest entity
        modelBuilder.Entity<PickRequest>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("PickRequestId");

            entity.Property(e => e.RequestNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.RequestNumber)
                .IsUnique()
                .HasDatabaseName("UQ_PickRequests_RequestNumber");

            entity.Property(e => e.OrderId);

            entity.HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .HasConstraintName("FK_PickRequests_Orders")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.ClientId)
                .IsRequired();

            entity.HasOne(e => e.Client)
                .WithMany()
                .HasForeignKey(e => e.ClientId)
                .HasConstraintName("FK_PickRequests_Clients")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.ClientLocationId);

            entity.HasOne(e => e.ClientLocation)
                .WithMany()
                .HasForeignKey(e => e.ClientLocationId)
                .HasConstraintName("FK_PickRequests_ClientLocations")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.RequestedBy);

            entity.HasOne(e => e.Requester)
                .WithMany()
                .HasForeignKey(e => e.RequestedBy)
                .HasConstraintName("FK_PickRequests_RequestedBy")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(e => e.ExecutionDate);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Created");

            entity.Property(e => e.DestinationAddress)
                .HasMaxLength(500);

            entity.Property(e => e.DestinationCity)
                .HasMaxLength(150);

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.DriverId);

            entity.HasOne(e => e.Driver)
                .WithMany()
                .HasForeignKey(e => e.DriverId)
                .HasConstraintName("FK_PickRequests_Driver")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.VehicleId);

            entity.HasOne(e => e.Vehicle)
                .WithMany()
                .HasForeignKey(e => e.VehicleId)
                .HasConstraintName("FK_PickRequests_Vehicle")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Verified)
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("PickRequests");
        });

        // Configure PickRequestLine entity
        modelBuilder.Entity<PickRequestLine>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("PickRequestLineId");

            entity.Property(e => e.PickRequestId)
                .IsRequired();

            entity.HasOne(e => e.PickRequest)
                .WithMany(p => p.PickRequestLines)
                .HasForeignKey(e => e.PickRequestId)
                .HasConstraintName("FK_PickRequestLines_PickRequests")
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.ProductVariantId)
                .IsRequired();

            entity.HasOne(e => e.ProductVariant)
                .WithMany()
                .HasForeignKey(e => e.ProductVariantId)
                .HasConstraintName("FK_PickRequestLines_ProductVariants")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.ToTable("PickRequestLines", t => t.HasCheckConstraint("CK_PickRequestLines_Quantity", "[Quantity] > 0"));

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // Configure PickRequestPart entity
        modelBuilder.Entity<PickRequestPart>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("PickRequestPartId");

            entity.Property(e => e.PickRequestId)
                .IsRequired();

            entity.HasOne(e => e.PickRequest)
                .WithMany(p => p.PickRequestParts)
                .HasForeignKey(e => e.PickRequestId)
                .HasConstraintName("FK_PickRequestParts_PickRequests")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.PickRequestLineId)
                .IsRequired();

            entity.HasOne(e => e.PickRequestLine)
                .WithMany(l => l.PickRequestParts)
                .HasForeignKey(e => e.PickRequestLineId)
                .HasConstraintName("FK_PickRequestParts_PickRequestLines")
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.PartId)
                .IsRequired();

            entity.HasOne(e => e.Part)
                .WithMany()
                .HasForeignKey(e => e.PartId)
                .HasConstraintName("FK_PickRequestParts_Parts")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.RequiredQuantity)
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(e => e.PickedQuantity)
                .HasPrecision(18, 4)
                .HasDefaultValue(0m);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.ToTable("PickRequestParts", t =>
            {
                t.HasCheckConstraint("CK_PickRequestParts_RequiredQuantity", "[RequiredQuantity] > 0");
                t.HasCheckConstraint("CK_PickRequestParts_PickedQuantity", "[PickedQuantity] >= 0");
            });

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // Configure Pick entity
        modelBuilder.Entity<Pick>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("PickId");

            entity.Property(e => e.PickRequestId)
                .IsRequired();

            entity.HasOne(e => e.PickRequest)
                .WithMany(p => p.Picks)
                .HasForeignKey(e => e.PickRequestId)
                .HasConstraintName("FK_Picks_PickRequests")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.PickRequestPartId);

            entity.HasOne(e => e.PickRequestPart)
                .WithMany(prp => prp.Picks)
                .HasForeignKey(e => e.PickRequestPartId)
                .HasConstraintName("FK_Picks_PickRequestParts")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.PartId)
                .IsRequired();

            entity.HasOne(e => e.Part)
                .WithMany()
                .HasForeignKey(e => e.PartId)
                .HasConstraintName("FK_Picks_Parts")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Barcode)
                .HasMaxLength(100);

            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(e => e.PickedBy);

            entity.HasOne(e => e.Picker)
                .WithMany()
                .HasForeignKey(e => e.PickedBy)
                .HasConstraintName("FK_Picks_PickedBy")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.DriverId);

            entity.HasOne(e => e.Driver)
                .WithMany()
                .HasForeignKey(e => e.DriverId)
                .HasConstraintName("FK_Picks_Driver")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.VehicleId);

            entity.HasOne(e => e.Vehicle)
                .WithMany()
                .HasForeignKey(e => e.VehicleId)
                .HasConstraintName("FK_Picks_Vehicle")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.PickDate)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Picked");

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            entity.ToTable("Picks", t =>
            {
                t.HasCheckConstraint("CK_Picks_Quantity", "[Quantity] > 0");
            });

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });
    }
}