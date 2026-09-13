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

    /// <summary>
    /// Status lookup DbSets
    /// </summary>
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<LoadRequestStatus> LoadRequestStatuses { get; set; }
    public DbSet<VehicleLoadStatus> VehicleLoadStatuses { get; set; }
    public DbSet<VehicleOffloadStatus> VehicleOffloadStatuses { get; set; }
    public DbSet<FieldJobStatus> FieldJobStatuses { get; set; }
    public DbSet<FieldAssemblyStatus> FieldAssemblyStatuses { get; set; }
    public DbSet<IssueStatus> IssueStatuses { get; set; }


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

            entity.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.NameAr)
                .HasMaxLength(200);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(1000);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(1000);

            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);

            entity.Property(e => e.Barcode)
                .HasMaxLength(100);

            entity.HasIndex(e => e.Barcode)
                .IsUnique()
                .HasDatabaseName("UQ_Products_Barcode");

            entity.Property(e => e.PictureUrl)
                .HasMaxLength(500);

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

            entity.Property(e => e.FullNameEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.FullNameAr)
                .HasMaxLength(200);

            entity.Ignore(e => e.FullName);

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

            entity.Property(e => e.PreferredLanguage)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("en");

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

            entity.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.NameAr)
                .HasMaxLength(150);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);

            entity.HasIndex(e => e.NameEn)
                .IsUnique()
                .HasDatabaseName("UQ_Categories_NameEn");

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

            entity.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.NameAr)
                .HasMaxLength(100);

            entity.Ignore(e => e.Name);

            entity.HasIndex(e => e.NameEn)
                .IsUnique()
                .HasDatabaseName("UQ_Colors_NameEn");

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

            entity.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.NameAr)
                .HasMaxLength(150);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);

            entity.HasIndex(e => e.NameEn)
                .IsUnique()
                .HasDatabaseName("UQ_Materials_NameEn");

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

            entity.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.NameAr)
                .HasMaxLength(150);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);

            entity.HasIndex(e => e.NameEn)
                .IsUnique()
                .HasDatabaseName("UQ_Designs_NameEn");

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

            entity.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.NameAr)
                .HasMaxLength(200);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(1000);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(1000);

            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);

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

        // Configure OrderStatus lookup entity
        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("OrderStatusId").ValueGeneratedNever();
            entity.Property(e => e.NameEn).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NameAr).HasMaxLength(50);
            entity.Property(e => e.DescriptionEn).HasMaxLength(250);
            entity.Property(e => e.DescriptionAr).HasMaxLength(250);
            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.ToTable("OrderStatuses");

            entity.HasData(
                new OrderStatus { Id = (int)OrderStatusEnum.Draft, NameEn = "Draft", NameAr = "مسودة", DescriptionEn = "Draft order created", DescriptionAr = "تم إنشاء أمر طلب مسودة", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new OrderStatus { Id = (int)OrderStatusEnum.Pending, NameEn = "Pending", NameAr = "قيد الانتظار", DescriptionEn = "Order pending approval", DescriptionAr = "أمر الطلب بانتظار الموافقة", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new OrderStatus { Id = (int)OrderStatusEnum.Approved, NameEn = "Approved", NameAr = "معتمد", DescriptionEn = "Order approved by sales manager", DescriptionAr = "تم اعتماد أمر الطلب من مدير المبيعات", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new OrderStatus { Id = (int)OrderStatusEnum.Processing, NameEn = "Processing", NameAr = "قيد المعالجة", DescriptionEn = "Load requests placed for order", DescriptionAr = "تم إنشاء طلبات التحميل لأمر الطلب", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new OrderStatus { Id = (int)OrderStatusEnum.Completed, NameEn = "Completed", NameAr = "مكتمل", DescriptionEn = "All load requests fulfilled and order completed", DescriptionAr = "تم تنفيذ جميع طلبات التحميل واكتمال أمر الطلب", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new OrderStatus { Id = (int)OrderStatusEnum.Cancelled, NameEn = "Cancelled", NameAr = "ملغي", DescriptionEn = "Order cancelled", DescriptionAr = "تم إلغاء أمر الطلب", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
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

            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValue((int)OrderStatusEnum.Draft);

            entity.HasOne(e => e.OrderStatus)
                .WithMany()
                .HasForeignKey(e => e.Status)
                .HasConstraintName("FK_Orders_OrderStatuses")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Status)
                .HasDatabaseName("IX_Orders_Status");

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

        // Configure LoadRequestStatus lookup entity
        modelBuilder.Entity<LoadRequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("LoadRequestStatusId").ValueGeneratedNever();
            entity.Property(e => e.NameEn).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NameAr).HasMaxLength(50);
            entity.Property(e => e.DescriptionEn).HasMaxLength(250);
            entity.Property(e => e.DescriptionAr).HasMaxLength(250);
            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.ToTable("LoadRequestStatuses");

            entity.HasData(
                new LoadRequestStatus { Id = (int)LoadRequestStatusEnum.New, NameEn = "New", NameAr = "جديد", DescriptionEn = "New load request created", DescriptionAr = "تم إنشاء طلب تحميل جديد", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new LoadRequestStatus { Id = (int)LoadRequestStatusEnum.Loaded, NameEn = "Loaded", NameAr = "تم التحميل", DescriptionEn = "Loading completed in good condition", DescriptionAr = "اكتمل التحميل بحالة جيدة", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new LoadRequestStatus { Id = (int)LoadRequestStatusEnum.Offloaded, NameEn = "Offloaded", NameAr = "تم التنزيل", DescriptionEn = "Offloaded in good condition at destination", DescriptionAr = "تم تنزيل الشحنة بحالة جيدة في الوجهة", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new LoadRequestStatus { Id = (int)LoadRequestStatusEnum.Completed, NameEn = "Completed", NameAr = "مكتمل", DescriptionEn = "All field assemblies completed", DescriptionAr = "اكتملت جميع أعمال التركيب الميداني", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new LoadRequestStatus { Id = (int)LoadRequestStatusEnum.Blocked, NameEn = "Blocked", NameAr = "محظور / معلق", DescriptionEn = "Blocked due to damaged or missing parts during loading/offloading", DescriptionAr = "معلق بسبب قطع تالفة أو مفقودة أثناء التحميل/التنزيل", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new LoadRequestStatus { Id = (int)LoadRequestStatusEnum.Cancelled, NameEn = "Cancelled", NameAr = "ملغي", DescriptionEn = "Load request cancelled", DescriptionAr = "تم إلغاء طلب التحميل", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
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
                .HasDefaultValue((int)LoadRequestStatusEnum.New);

            entity.HasOne(e => e.LoadRequestStatus)
                .WithMany()
                .HasForeignKey(e => e.Status)
                .HasConstraintName("FK_LoadRequests_LoadRequestStatuses")
                .OnDelete(DeleteBehavior.Restrict);

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

            entity.Property(e => e.ProductId)
                .IsRequired();

            entity.HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .HasConstraintName("FK_LoadRequestParts_Products")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ProductId)
                .HasDatabaseName("IX_LoadRequestParts_ProductId");

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

            entity.ToTable("LoadRequestParts", t =>
            {
                t.HasCheckConstraint("CK_LoadRequestParts_RequiredQuantity", "[RequiredQuantity] > 0");
                t.HasCheckConstraint("CK_LoadRequestParts_LoadedQuantity", "[LoadedQuantity] >= 0");
            });

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // Configure VehicleLoadStatus lookup entity
        modelBuilder.Entity<VehicleLoadStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("VehicleLoadStatusId").ValueGeneratedNever();
            entity.Property(e => e.NameEn).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NameAr).HasMaxLength(50);
            entity.Property(e => e.DescriptionEn).HasMaxLength(250);
            entity.Property(e => e.DescriptionAr).HasMaxLength(250);
            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.ToTable("VehicleLoadStatuses");

            entity.HasData(
                new VehicleLoadStatus { Id = (int)VehicleLoadStatusEnum.Good, NameEn = "Good", NameAr = "سليم", DescriptionEn = "Loaded in good condition", DescriptionAr = "تم التحميل بحالة سليمة وجيدة", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new VehicleLoadStatus { Id = (int)VehicleLoadStatusEnum.Damaged, NameEn = "Damaged", NameAr = "تالف", DescriptionEn = "Loaded in damaged condition", DescriptionAr = "تم التحميل مع وجود تلف", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new VehicleLoadStatus { Id = (int)VehicleLoadStatusEnum.Missing, NameEn = "Missing", NameAr = "مفقود", DescriptionEn = "Missing items during load", DescriptionAr = "عناصر مفقودة أثناء التحميل", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
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
                .HasDefaultValue((int)VehicleLoadStatusEnum.Good);

            entity.HasOne(e => e.VehicleLoadStatus)
                .WithMany()
                .HasForeignKey(e => e.Status)
                .HasConstraintName("FK_VehicleLoads_VehicleLoadStatuses")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Status)
                .HasDatabaseName("IX_VehicleLoads_Status");

            entity.Property(e => e.Notes)
                .HasMaxLength(500);

            entity.ToTable("VehicleLoads", t =>
            {
                t.HasCheckConstraint("CK_VehicleLoads_Quantity", "[Quantity] > 0");
            });

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()");
        });

        // Configure VehicleOffloadStatus lookup entity
        modelBuilder.Entity<VehicleOffloadStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("VehicleOffloadStatusId").ValueGeneratedNever();
            entity.Property(e => e.NameEn).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NameAr).HasMaxLength(50);
            entity.Property(e => e.DescriptionEn).HasMaxLength(250);
            entity.Property(e => e.DescriptionAr).HasMaxLength(250);
            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.ToTable("VehicleOffloadStatuses");

            entity.HasData(
                new VehicleOffloadStatus { Id = (int)VehicleOffloadStatusEnum.Good, NameEn = "Good", NameAr = "سليم", DescriptionEn = "Offloaded in good condition", DescriptionAr = "تم التنزيل بحالة سليمة وجيدة", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new VehicleOffloadStatus { Id = (int)VehicleOffloadStatusEnum.Damaged, NameEn = "Damaged", NameAr = "تالف", DescriptionEn = "Offloaded in damaged condition", DescriptionAr = "تم التنزيل مع وجود تلف", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new VehicleOffloadStatus { Id = (int)VehicleOffloadStatusEnum.Missing, NameEn = "Missing", NameAr = "مفقود", DescriptionEn = "Missing items during offload", DescriptionAr = "عناصر مفقودة أثناء التنزيل", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
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
                .HasDefaultValue((int)VehicleOffloadStatusEnum.Good);

            entity.HasOne(e => e.VehicleOffloadStatus)
                .WithMany()
                .HasForeignKey(e => e.Status)
                .HasConstraintName("FK_VehicleOffloads_VehicleOffloadStatuses")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Status)
                .HasDatabaseName("IX_VehicleOffloads_Status");

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

        // Configure FieldJobStatus lookup entity
        modelBuilder.Entity<FieldJobStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("FieldJobStatusId").ValueGeneratedNever();
            entity.Property(e => e.NameEn).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NameAr).HasMaxLength(50);
            entity.Property(e => e.DescriptionEn).HasMaxLength(250);
            entity.Property(e => e.DescriptionAr).HasMaxLength(250);
            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.ToTable("FieldJobStatuses");

            entity.HasData(
                new FieldJobStatus { Id = (int)FieldJobStatusEnum.Scheduled, NameEn = "Scheduled", NameAr = "مجدول", DescriptionEn = "Field job scheduled", DescriptionAr = "تمت جدولة المهمة الميدانية", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new FieldJobStatus { Id = (int)FieldJobStatusEnum.InProgress, NameEn = "InProgress", NameAr = "قيد التنفيذ", DescriptionEn = "Field job in progress", DescriptionAr = "المهمة الميدانية قيد التنفيذ", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new FieldJobStatus { Id = (int)FieldJobStatusEnum.Completed, NameEn = "Completed", NameAr = "مكتمل", DescriptionEn = "Field job completed", DescriptionAr = "تم اكتمال المهمة الميدانية بنجاح", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new FieldJobStatus { Id = (int)FieldJobStatusEnum.Cancelled, NameEn = "Cancelled", NameAr = "ملغي", DescriptionEn = "Field job cancelled", DescriptionAr = "تم إلغاء المهمة الميدانية", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
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
                .HasDefaultValue((int)FieldJobStatusEnum.Scheduled);

            entity.HasOne(e => e.FieldJobStatus)
                .WithMany()
                .HasForeignKey(e => e.Status)
                .HasConstraintName("FK_FieldJobs_FieldJobStatuses")
                .OnDelete(DeleteBehavior.Restrict);

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

        // Configure FieldAssemblyStatus lookup entity
        modelBuilder.Entity<FieldAssemblyStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("FieldAssemblyStatusId").ValueGeneratedNever();
            entity.Property(e => e.NameEn).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NameAr).HasMaxLength(50);
            entity.Property(e => e.DescriptionEn).HasMaxLength(250);
            entity.Property(e => e.DescriptionAr).HasMaxLength(250);
            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.ToTable("FieldAssemblyStatuses");

            entity.HasData(
                new FieldAssemblyStatus { Id = (int)FieldAssemblyStatusEnum.InProgress, NameEn = "InProgress", NameAr = "قيد التنفيذ", DescriptionEn = "Assembly in progress", DescriptionAr = "أعمال التركيب قيد التنفيذ", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new FieldAssemblyStatus { Id = (int)FieldAssemblyStatusEnum.Completed, NameEn = "Completed", NameAr = "مكتمل", DescriptionEn = "Assembly completed", DescriptionAr = "تم اكتمال أعمال التركيب", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new FieldAssemblyStatus { Id = (int)FieldAssemblyStatusEnum.Cancelled, NameEn = "Cancelled", NameAr = "ملغي", DescriptionEn = "Assembly cancelled", DescriptionAr = "تم إلغاء أعمال التركيب", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
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
                .HasDefaultValue((int)FieldAssemblyStatusEnum.InProgress);

            entity.HasOne(e => e.FieldAssemblyStatus)
                .WithMany()
                .HasForeignKey(e => e.Status)
                .HasConstraintName("FK_FieldAssemblies_FieldAssemblyStatuses")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.Status)
                .HasDatabaseName("IX_FieldAssemblies_Status");

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

        // Configure IssueStatus lookup entity
        modelBuilder.Entity<IssueStatus>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("IssueStatusId").ValueGeneratedNever();
            entity.Property(e => e.NameEn).IsRequired().HasMaxLength(50);
            entity.Property(e => e.NameAr).HasMaxLength(50);
            entity.Property(e => e.DescriptionEn).HasMaxLength(250);
            entity.Property(e => e.DescriptionAr).HasMaxLength(250);
            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.ToTable("IssueStatuses");

            entity.HasData(
                new IssueStatus { Id = (int)IssueStatusEnum.Open, NameEn = "Open", NameAr = "مفتوح", DescriptionEn = "Issue open and unresolved", DescriptionAr = "المشكلة مفتوحة وغير محلولة", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new IssueStatus { Id = (int)IssueStatusEnum.InProgress, NameEn = "InProgress", NameAr = "قيد المعالجة", DescriptionEn = "Issue in progress of investigation/fixing", DescriptionAr = "المشكلة قيد الفحص والحل", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new IssueStatus { Id = (int)IssueStatusEnum.Resolved, NameEn = "Resolved", NameAr = "تم الحل", DescriptionEn = "Issue resolved", DescriptionAr = "تم حل المشكلة", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new IssueStatus { Id = (int)IssueStatusEnum.Closed, NameEn = "Closed", NameAr = "مغلق", DescriptionEn = "Issue closed", DescriptionAr = "تم إغلاق المشكلة", CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
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
                .HasDefaultValue((int)IssueStatusEnum.Open);

            entity.HasOne(e => e.IssueStatus)
                .WithMany()
                .HasForeignKey(e => e.Status)
                .HasConstraintName("FK_Issues_IssueStatuses")
                .OnDelete(DeleteBehavior.Restrict);

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

            entity.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.NameAr)
                .HasMaxLength(100);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(500);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(500);

            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);

            entity.HasIndex(e => e.NameEn)
                .IsUnique()
                .HasDatabaseName("UQ_Roles_NameEn");

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

            entity.Property(e => e.NameEn)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.NameAr)
                .HasMaxLength(100);

            entity.Property(e => e.DescriptionEn)
                .HasMaxLength(250);

            entity.Property(e => e.DescriptionAr)
                .HasMaxLength(250);

            entity.Ignore(e => e.Name);
            entity.Ignore(e => e.Description);

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Module)
                .HasMaxLength(50);

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasIndex(e => e.Code)
                .IsUnique()
                .HasDatabaseName("UQ_Screens_Code");

            entity.HasIndex(e => e.NameEn)
                .IsUnique()
                .HasDatabaseName("UQ_Screens_NameEn");

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