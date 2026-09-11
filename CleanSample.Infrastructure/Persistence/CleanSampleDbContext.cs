using CleanSample.Domain.Entities;
using CleanSample.Domain.Enums;
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
    /// LoadRequests DbSet
    /// </summary>
    public DbSet<LoadRequest> LoadRequests { get; set; }

    /// <summary>
    /// LoadRequestLines DbSet
    /// </summary>
    public DbSet<LoadRequestLine> LoadRequestLines { get; set; }

    /// <summary>
    /// LoadRequestParts DbSet
    /// </summary>
    public DbSet<LoadRequestPart> LoadRequestParts { get; set; }

    /// <summary>
    /// VehicleLoads DbSet
    /// </summary>
    public DbSet<VehicleLoad> VehicleLoads { get; set; }

    /// <summary>
    /// VehicleOffloads DbSet
    /// </summary>
    public DbSet<VehicleOffload> VehicleOffloads { get; set; }

    /// <summary>
    /// FieldJobs DbSet
    /// </summary>
    public DbSet<FieldJob> FieldJobs { get; set; }

    /// <summary>
    /// FieldAssemblies DbSet
    /// </summary>
    public DbSet<FieldAssembly> FieldAssemblies { get; set; }

    /// <summary>
    /// Issues DbSet
    /// </summary>
    public DbSet<Issue> Issues { get; set; }

    /// <summary>
    /// Roles DbSet
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Screens DbSet
    /// </summary>
    public DbSet<Screen> Screens { get; set; }

    /// <summary>
    /// RolePermissions DbSet
    /// </summary>
    public DbSet<RolePermission> RolePermissions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("ProductId");

            entity.Property(e => e.CategoryId)
                .IsRequired();

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .HasConstraintName("FK_Products_Categories")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Color)
                .WithMany()
                .HasForeignKey(e => e.ColorId)
                .HasConstraintName("FK_Products_Colors")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Material)
                .WithMany()
                .HasForeignKey(e => e.MaterialId)
                .HasConstraintName("FK_Products_Materials")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Design)
                .WithMany()
                .HasForeignKey(e => e.DesignId)
                .HasConstraintName("FK_Products_Designs")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Barcode)
                .HasMaxLength(100);

            entity.HasIndex(e => e.Barcode)
                .IsUnique()
                .HasDatabaseName("UQ_Products_Barcode");

            entity.Property(e => e.Description)
                .HasMaxLength(1000);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("Products");
        });

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("UserId");

            entity.Property(e => e.RoleId);

            entity.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .HasConstraintName("FK_Users_Roles")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(e => e.UserName)
                .IsUnique()
                .HasDatabaseName("UQ_Users_UserName");

            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Email)
                .HasMaxLength(250);

            entity.Property(e => e.Mobile)
                .HasMaxLength(50);

            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(500)
                .HasDefaultValue(string.Empty);

            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasMaxLength(250)
                .HasDefaultValue(string.Empty);

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Ignore(e => e.Username);

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

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("Designs");
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

            entity.Property(e => e.ProductId)
                .IsRequired();

            entity.Property(e => e.PartId)
                .IsRequired();

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .HasConstraintName("FK_ProductBOM_Products")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Part)
                .WithMany()
                .HasForeignKey(e => e.PartId)
                .HasConstraintName("FK_ProductBOM_Parts")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.ProductId, e.PartId })
                .IsUnique()
                .HasDatabaseName("UQ_ProductBOM_Product_Part");

            entity.HasIndex(e => e.ProductId)
                .HasDatabaseName("IX_ProductBOM_ProductId");

            entity.HasIndex(e => e.PartId)
                .HasDatabaseName("IX_ProductBOM_PartId");

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


            entity.Property(e => e.ClientId)
                .IsRequired();

            entity.HasOne(e => e.Client)
                .WithMany()
                .HasForeignKey(e => e.ClientId)
                .HasConstraintName("FK_Orders_Clients")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ClientId)
                .HasDatabaseName("IX_Orders_ClientId");

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

            entity.Property(e => e.ProductId)
                .IsRequired();

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .HasConstraintName("FK_OrderLines_Products")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.OrderId)
                .HasDatabaseName("IX_OrderLines_OrderId");

            entity.HasIndex(e => e.ProductId)
                .HasDatabaseName("IX_OrderLines_ProductId");

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.ToTable("OrderLines", t => t.HasCheckConstraint("CK_OrderLines_Quantity", "[Quantity] > 0"));

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // Configure LoadRequest entity
        modelBuilder.Entity<LoadRequest>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("LoadRequestId");


            entity.Property(e => e.OrderId);

            entity.HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .HasConstraintName("FK_LoadRequests_Orders")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.OrderId)
                .HasDatabaseName("IX_LoadRequests_OrderId");

            entity.Property(e => e.ClientId)
                .IsRequired();

            entity.HasOne(e => e.Client)
                .WithMany()
                .HasForeignKey(e => e.ClientId)
                .HasConstraintName("FK_LoadRequests_Clients")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ClientId)
                .HasDatabaseName("IX_LoadRequests_ClientId");

            entity.Property(e => e.ClientLocationId);

            entity.HasOne(e => e.ClientLocation)
                .WithMany()
                .HasForeignKey(e => e.ClientLocationId)
                .HasConstraintName("FK_LoadRequests_ClientLocations")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.RequestedBy);

            entity.HasOne(e => e.Requester)
                .WithMany()
                .HasForeignKey(e => e.RequestedBy)
                .HasConstraintName("FK_LoadRequests_RequestedBy")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(e => e.ExecutionDate);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValue(LoadRequestStatus.New);

            entity.HasIndex(e => e.Status)
                .HasDatabaseName("IX_LoadRequests_Status");

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
                .HasConstraintName("FK_LoadRequests_Driver")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.VehicleId);

            entity.HasOne(e => e.Vehicle)
                .WithMany()
                .HasForeignKey(e => e.VehicleId)
                .HasConstraintName("FK_LoadRequests_Vehicle")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Verified)
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            // Add table name
            entity.ToTable("LoadRequests");
        });

        // Configure LoadRequestLine entity
        modelBuilder.Entity<LoadRequestLine>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("LoadRequestLineId");

            entity.Property(e => e.LoadRequestId)
                .IsRequired();

            entity.HasOne(e => e.LoadRequest)
                .WithMany(p => p.LoadRequestLines)
                .HasForeignKey(e => e.LoadRequestId)
                .HasConstraintName("FK_LoadRequestLines_LoadRequests")
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.ProductId)
                .IsRequired();

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .HasConstraintName("FK_LoadRequestLines_Products")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.ToTable("LoadRequestLines", t => t.HasCheckConstraint("CK_LoadRequestLines_Quantity", "[Quantity] > 0"));

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // Configure LoadRequestPart entity
        modelBuilder.Entity<LoadRequestPart>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("LoadRequestPartId");

            entity.Property(e => e.LoadRequestId)
                .IsRequired();

            entity.HasOne(e => e.LoadRequest)
                .WithMany(p => p.LoadRequestParts)
                .HasForeignKey(e => e.LoadRequestId)
                .HasConstraintName("FK_LoadRequestParts_LoadRequests")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.LoadRequestId)
                .HasDatabaseName("IX_LoadRequestParts_LoadRequestId");

            entity.Property(e => e.LoadRequestLineId)
                .IsRequired();

            entity.HasOne(e => e.LoadRequestLine)
                .WithMany(l => l.LoadRequestParts)
                .HasForeignKey(e => e.LoadRequestLineId)
                .HasConstraintName("FK_LoadRequestParts_LoadRequestLines")
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.PartId)
                .IsRequired();

            entity.HasOne(e => e.Part)
                .WithMany()
                .HasForeignKey(e => e.PartId)
                .HasConstraintName("FK_LoadRequestParts_Parts")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.PartId)
                .HasDatabaseName("IX_LoadRequestParts_PartId");

            entity.Property(e => e.RequiredQuantity)
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(e => e.LoadedQuantity)
                .HasPrecision(18, 4)
                .HasDefaultValue(0m);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.ToTable("LoadRequestParts", t =>
            {
                t.HasCheckConstraint("CK_LoadRequestParts_RequiredQuantity", "[RequiredQuantity] > 0");
                t.HasCheckConstraint("CK_LoadRequestParts_LoadedQuantity", "[LoadedQuantity] >= 0");
            });

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // Configure VehicleLoad entity
        modelBuilder.Entity<VehicleLoad>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("LoadId");

            entity.Property(e => e.LoadRequestId)
                .IsRequired();

            entity.HasOne(e => e.LoadRequest)
                .WithMany(p => p.VehicleLoads)
                .HasForeignKey(e => e.LoadRequestId)
                .HasConstraintName("FK_VehicleLoads_LoadRequests")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.LoadRequestId)
                .HasDatabaseName("IX_VehicleLoads_LoadRequestId");

            entity.Property(e => e.PartId)
                .IsRequired();

            entity.HasOne(e => e.Part)
                .WithMany()
                .HasForeignKey(e => e.PartId)
                .HasConstraintName("FK_VehicleLoads_Parts")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Barcode)
                .HasMaxLength(100);

            entity.HasIndex(e => e.Barcode)
                .HasDatabaseName("IX_VehicleLoads_Barcode");

            entity.Property(e => e.Quantity)
                .HasPrecision(18, 4)
                .IsRequired();

            entity.Property(e => e.LoadedBy);

            entity.HasOne(e => e.Loader)
                .WithMany()
                .HasForeignKey(e => e.LoadedBy)
                .HasConstraintName("FK_VehicleLoads_LoadedBy")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.DriverId);

            entity.HasOne(e => e.Driver)
                .WithMany()
                .HasForeignKey(e => e.DriverId)
                .HasConstraintName("FK_VehicleLoads_Driver")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.VehicleId);

            entity.HasOne(e => e.Vehicle)
                .WithMany()
                .HasForeignKey(e => e.VehicleId)
                .HasConstraintName("FK_VehicleLoads_Vehicle")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.LoadDate)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Loaded");

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            entity.ToTable("VehicleLoads", t =>
            {
                t.HasCheckConstraint("CK_VehicleLoads_Quantity", "[Quantity] > 0");
            });

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // Configure VehicleOffload entity
        modelBuilder.Entity<VehicleOffload>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("VehicleOffloadId");

            entity.Property(e => e.LoadRequestId)
                .IsRequired();

            entity.HasOne(e => e.LoadRequest)
                .WithMany(p => p.VehicleOffloads)
                .HasForeignKey(e => e.LoadRequestId)
                .HasConstraintName("FK_VehicleOffloads_LoadRequests")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.LoadRequestId)
                .HasDatabaseName("IX_VehicleOffloads_LoadRequestId");

            entity.Property(e => e.PartId)
                .IsRequired();

            entity.HasOne(e => e.Part)
                .WithMany()
                .HasForeignKey(e => e.PartId)
                .HasConstraintName("FK_VehicleOffloads_Parts")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.PartId)
                .HasDatabaseName("IX_VehicleOffloads_PartId");

            entity.Property(e => e.VehicleId)
                .IsRequired();

            entity.HasOne(e => e.Vehicle)
                .WithMany()
                .HasForeignKey(e => e.VehicleId)
                .HasConstraintName("FK_VehicleOffloads_Vehicles")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.DriverId)
                .IsRequired();

            entity.HasOne(e => e.Driver)
                .WithMany()
                .HasForeignKey(e => e.DriverId)
                .HasConstraintName("FK_VehicleOffloads_Drivers")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Barcode)
                .HasMaxLength(100);

            entity.HasIndex(e => e.Barcode)
                .HasDatabaseName("IX_VehicleOffloads_Barcode");

            entity.Property(e => e.OffloadDate)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Offloading");

            entity.Property(e => e.Verified)
                .HasDefaultValue(false);

            entity.Property(e => e.VerifiedBy);

            entity.HasOne(e => e.Verifier)
                .WithMany()
                .HasForeignKey(e => e.VerifiedBy)
                .HasConstraintName("FK_VehicleOffloads_VerifiedBy")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.VerifiedAt);

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.ToTable("VehicleOffloads");
        });

        // Configure FieldJob entity
        modelBuilder.Entity<FieldJob>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("FieldJobId");


            entity.Property(e => e.LoadRequestId)
                .IsRequired();

            entity.HasOne(e => e.LoadRequest)
                .WithMany(p => p.FieldJobs)
                .HasForeignKey(e => e.LoadRequestId)
                .HasConstraintName("FK_FieldJobs_LoadRequests")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.LoadRequestId)
                .HasDatabaseName("IX_FieldJobs_LoadRequestId");

            entity.Property(e => e.ClientId)
                .IsRequired();

            entity.HasOne(e => e.Client)
                .WithMany(c => c.FieldJobs)
                .HasForeignKey(e => e.ClientId)
                .HasConstraintName("FK_FieldJobs_Clients")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.ClientLocationId);

            entity.HasOne(e => e.ClientLocation)
                .WithMany(cl => cl.FieldJobs)
                .HasForeignKey(e => e.ClientLocationId)
                .HasConstraintName("FK_FieldJobs_ClientLocations")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.TechnicianId);

            entity.HasOne(e => e.Technician)
                .WithMany()
                .HasForeignKey(e => e.TechnicianId)
                .HasConstraintName("FK_FieldJobs_Technician")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.SupervisorId);

            entity.HasOne(e => e.Supervisor)
                .WithMany()
                .HasForeignKey(e => e.SupervisorId)
                .HasConstraintName("FK_FieldJobs_Supervisor")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.ScheduledDate);
            entity.Property(e => e.StartDate);
            entity.Property(e => e.CompletionDate);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Scheduled");

            entity.HasIndex(e => e.Status)
                .HasDatabaseName("IX_FieldJobs_Status");

            entity.Property(e => e.Verified)
                .HasDefaultValue(false);

            entity.Property(e => e.VerifiedAt);

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.ToTable("FieldJobs");
        });

        // Configure FieldAssembly entity
        modelBuilder.Entity<FieldAssembly>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("FieldAssemblyId");

            entity.Property(e => e.FieldJobId)
                .IsRequired();

            entity.HasOne(e => e.FieldJob)
                .WithMany(fj => fj.FieldAssemblies)
                .HasForeignKey(e => e.FieldJobId)
                .HasConstraintName("FK_FieldAssemblies_FieldJobs")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.FieldJobId)
                .HasDatabaseName("IX_FieldAssemblies_FieldJobId");

            entity.Property(e => e.ProductId)
                .IsRequired();

            entity.HasOne(e => e.Product)
                .WithMany(p => p.FieldAssemblies)
                .HasForeignKey(e => e.ProductId)
                .HasConstraintName("FK_FieldAssemblies_Products")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ProductId)
                .HasDatabaseName("IX_FieldAssemblies_ProductId");

            entity.Property(e => e.ProductBarcode)
                .HasMaxLength(100);

            entity.Property(e => e.Quantity)
                .IsRequired();

            entity.Property(e => e.AssemblyDate);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.Property(e => e.TechnicianId);

            entity.HasOne(e => e.Technician)
                .WithMany()
                .HasForeignKey(e => e.TechnicianId)
                .HasConstraintName("FK_FieldAssemblies_Technician")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.SupervisorId);

            entity.HasOne(e => e.Supervisor)
                .WithMany()
                .HasForeignKey(e => e.SupervisorId)
                .HasConstraintName("FK_FieldAssemblies_Supervisor")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.Verified)
                .HasDefaultValue(false);

            entity.Property(e => e.VerifiedAt);

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.ToTable("FieldAssemblies", t =>
            {
                t.HasCheckConstraint("CK_FieldAssemblies_Quantity", "[Quantity] > 0");
            });
        });

        // Configure Issue entity
        modelBuilder.Entity<Issue>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("IssueId");

            entity.Property(e => e.LoadRequestId);

            entity.HasOne(e => e.LoadRequest)
                .WithMany(p => p.Issues)
                .HasForeignKey(e => e.LoadRequestId)
                .HasConstraintName("FK_Issues_LoadRequests")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.FieldJobId);

            entity.HasOne(e => e.FieldJob)
                .WithMany(fj => fj.Issues)
                .HasForeignKey(e => e.FieldJobId)
                .HasConstraintName("FK_Issues_FieldJobs")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.FieldAssemblyId);

            entity.HasOne(e => e.FieldAssembly)
                .WithMany(fa => fa.Issues)
                .HasForeignKey(e => e.FieldAssemblyId)
                .HasConstraintName("FK_Issues_FieldAssemblies")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.IssueType)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(e => e.Severity)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Medium");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Open");

            entity.HasIndex(e => e.Status)
                .HasDatabaseName("IX_Issues_Status");

            entity.Property(e => e.ReportedBy);

            entity.HasOne(e => e.ReportedByUser)
                .WithMany()
                .HasForeignKey(e => e.ReportedBy)
                .HasConstraintName("FK_Issues_ReportedBy")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.ReportedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.Property(e => e.ResolvedBy);

            entity.HasOne(e => e.ResolvedByUser)
                .WithMany()
                .HasForeignKey(e => e.ResolvedBy)
                .HasConstraintName("FK_Issues_ResolvedBy")
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.ResolvedAt);

            entity.Property(e => e.ResolutionNotes)
                .HasMaxLength(2000);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.ToTable("Issues");
        });

        // Configure Role entity
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("RoleId");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Roles_Name");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.ToTable("Roles");
        });

        // Configure Screen entity
        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("ScreenId");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Module)
                .HasMaxLength(50);

            entity.Property(e => e.Description)
                .HasMaxLength(250);

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasIndex(e => e.Code)
                .IsUnique()
                .HasDatabaseName("UQ_Screens_Code");

            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Screens_Name");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.ToTable("Screens");
        });

        // Configure RolePermission entity
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("RolePermissionId");

            entity.Property(e => e.RoleId)
                .IsRequired();

            entity.Property(e => e.ScreenId)
                .IsRequired();

            entity.Property(e => e.CanView)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.CanCreate)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.CanUpdate)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.CanDelete)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasIndex(e => new { e.RoleId, e.ScreenId })
                .IsUnique()
                .HasDatabaseName("UQ_RolePermissions_Role_Screen");

            entity.HasIndex(e => e.RoleId)
                .HasDatabaseName("IX_RolePermissions_RoleId");

            entity.HasIndex(e => e.ScreenId)
                .HasDatabaseName("IX_RolePermissions_ScreenId");

            entity.HasOne(e => e.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                .HasConstraintName("FK_RolePermissions_Roles")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Screen)
                .WithMany(s => s.RolePermissions)
                .HasForeignKey(e => e.ScreenId)
                .HasConstraintName("FK_RolePermissions_Screens")
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");

            entity.ToTable("RolePermissions");
        });
    }
}