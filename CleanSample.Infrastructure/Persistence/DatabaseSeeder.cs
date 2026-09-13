using CleanSample.Domain.Entities;
using CleanSample.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

/// <summary>
/// Database seeder for populating all tables with realistic Point of Sale Materials (POSM)
/// data for the Egyptian market (Cairo & Giza), complete with bilingual (Arabic & English) support.
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds the database with initial data for all tables
    /// </summary>
    public static async Task SeedAsync(CleanSampleDbContext context)
    {
        try
        {
            // 0. Clean up any legacy dummy / computer data if present
            await CleanupLegacyDataAsync(context);

            // 1. Roles
            await SeedRolesAsync(context);

            // 2. Users
            await SeedUsersAsync(context);

            // 3. Lookup Tables (Categories, Colors, Materials, Designs)
            await SeedCategoriesAsync(context);
            await SeedColorsAsync(context);
            await SeedMaterialsAsync(context);
            await SeedDesignsAsync(context);

            // 4. POSM Products
            await SeedProductsAsync(context);

            // 5. POSM Parts & ProductBOM
            await SeedPartsAsync(context);
            await SeedProductBOMAsync(context);

            // 6. Vehicles (Egyptian Fleet)
            await SeedVehiclesAsync(context);

            // 7. Clients & ClientLocations (Cairo & Giza Retailers)
            await SeedClientsAsync(context);
            await SeedClientLocationsAsync(context);

            // 8. Orders & OrderLines
            await SeedOrdersAsync(context);
            await SeedOrderLinesAsync(context);

            // 9. LoadRequests, LoadRequestLines, LoadRequestParts
            await SeedLoadRequestsAsync(context);
            await SeedLoadRequestLinesAsync(context);
            await SeedLoadRequestPartsAsync(context);

            // 10. VehicleLoads
            await SeedVehicleLoadsAsync(context);

            // 11. VehicleOffloads
            await SeedVehicleOffloadsAsync(context);

            // 12. FieldJobs & FieldAssemblies
            await SeedFieldJobsAsync(context);
            await SeedFieldAssembliesAsync(context);

            // 13. Issues
            await SeedIssuesAsync(context);

            // 14. Screens & RolePermissions
            await SeedScreensAsync(context);
            await SeedRolePermissionsAsync(context);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error occurred while seeding the database", ex);
        }
    }

    private static async Task CleanupLegacyDataAsync(CleanSampleDbContext context)
    {
        // Detect if database has old dummy data (e.g. computer laptop items or New York clients)
        var hasLegacyData = await context.Clients.AnyAsync(c => c.City == "New York" || c.Name.Contains("Apex Technology Solutions"))
            || await context.Products.AnyAsync(p => p.NameEn.Contains("Laptop") || p.NameEn.Contains("Smart Phone"));

        if (hasLegacyData)
        {
            // Remove in reverse foreign key order
            context.Issues.RemoveRange(context.Issues);
            context.FieldAssemblies.RemoveRange(context.FieldAssemblies);
            context.FieldJobs.RemoveRange(context.FieldJobs);
            context.VehicleOffloads.RemoveRange(context.VehicleOffloads);
            context.VehicleLoads.RemoveRange(context.VehicleLoads);
            context.LoadRequestParts.RemoveRange(context.LoadRequestParts);
            context.LoadRequestLines.RemoveRange(context.LoadRequestLines);
            context.LoadRequests.RemoveRange(context.LoadRequests);
            context.OrderLines.RemoveRange(context.OrderLines);
            context.Orders.RemoveRange(context.Orders);
            context.ClientLocations.RemoveRange(context.ClientLocations);
            context.Clients.RemoveRange(context.Clients);
            context.ProductBOMs.RemoveRange(context.ProductBOMs);
            context.Products.RemoveRange(context.Products);
            context.Parts.RemoveRange(context.Parts);
            context.Vehicles.RemoveRange(context.Vehicles);
            context.Categories.RemoveRange(context.Categories);
            context.Materials.RemoveRange(context.Materials);
            context.Designs.RemoveRange(context.Designs);
            context.Colors.RemoveRange(context.Colors);

            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedRolesAsync(CleanSampleDbContext context)
    {
        if (await context.Roles.AnyAsync()) return;

        var roles = new List<Role>
        {
            new Role { NameEn = "Admin", NameAr = "مدير النظام", DescriptionEn = "System Administrator with full access", DescriptionAr = "مدير النظام بكامل الصلاحيات" },
            new Role { NameEn = "User", NameAr = "مستخدم", DescriptionEn = "Standard system user", DescriptionAr = "مستخدم نظام قياسي" },
            new Role { NameEn = "Manager", NameAr = "مدير", DescriptionEn = "Operations and sales manager", DescriptionAr = "مدير العمليات والمبيعات" },
            new Role { NameEn = "Driver", NameAr = "سائق", DescriptionEn = "Logistics fleet driver", DescriptionAr = "سائق أسطول الخدمات اللوجستية" },
            new Role { NameEn = "Technician", NameAr = "فني", DescriptionEn = "Field service technician", DescriptionAr = "فني الخدمات والتركيبات الميدانية" },
            new Role { NameEn = "Supervisor", NameAr = "مشرف", DescriptionEn = "Operations field supervisor", DescriptionAr = "مشرف العمليات الميدانية" }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(CleanSampleDbContext context)
    {
        var (defaultHash, defaultSalt) = HashPasswordWithSalt("P@$$w0rd");

        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.NameEn == "Admin" || r.NameAr == "مدير النظام");
        var userRole = await context.Roles.FirstOrDefaultAsync(r => r.NameEn == "User" || r.NameAr == "مستخدم");
        var managerRole = await context.Roles.FirstOrDefaultAsync(r => r.NameEn == "Manager" || r.NameAr == "مدير");
        var driverRole = await context.Roles.FirstOrDefaultAsync(r => r.NameEn == "Driver" || r.NameAr == "سائق");
        var technicianRole = await context.Roles.FirstOrDefaultAsync(r => r.NameEn == "Technician" || r.NameAr == "فني");
        var supervisorRole = await context.Roles.FirstOrDefaultAsync(r => r.NameEn == "Supervisor" || r.NameAr == "مشرف");

        // Check if admin user exists; if not or if old, ensure Egyptian users are created
        var usersToSeed = new List<User>
        {
            new User
            {
                UserName = "admin",
                Email = "admin@posm-egypt.com",
                FullNameEn = "Karim El-Sayed",
                FullNameAr = "كريم السيد",
                RoleId = adminRole?.Id,
                Mobile = "+20 100 111 0001",
                PasswordHash = defaultHash,
                PasswordSalt = defaultSalt,
                PreferredLanguage = "en",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                UserName = "manager",
                Email = "sherif.manager@posm-egypt.com",
                FullNameEn = "Sherif Abdelrahman",
                FullNameAr = "شريف عبد الرحمن",
                RoleId = managerRole?.Id,
                Mobile = "+20 100 222 0002",
                PasswordHash = defaultHash,
                PasswordSalt = defaultSalt,
                PreferredLanguage = "ar",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                UserName = "supervisor_tarek",
                Email = "tarek.supervisor@posm-egypt.com",
                FullNameEn = "Tarek El-Gohary",
                FullNameAr = "طارق الجوهري",
                RoleId = supervisorRole?.Id,
                Mobile = "+20 100 333 0003",
                PasswordHash = defaultHash,
                PasswordSalt = defaultSalt,
                PreferredLanguage = "ar",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                UserName = "driver_ahmed",
                Email = "ahmed.driver@posm-egypt.com",
                FullNameEn = "Ahmed Hassan",
                FullNameAr = "أحمد حسن",
                RoleId = driverRole?.Id,
                Mobile = "+20 111 444 0004",
                PasswordHash = defaultHash,
                PasswordSalt = defaultSalt,
                PreferredLanguage = "ar",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                UserName = "driver_mahmoud",
                Email = "mahmoud.driver@posm-egypt.com",
                FullNameEn = "Mahmoud Taha",
                FullNameAr = "محمود طه",
                RoleId = driverRole?.Id,
                Mobile = "+20 111 555 0005",
                PasswordHash = defaultHash,
                PasswordSalt = defaultSalt,
                PreferredLanguage = "ar",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                UserName = "tech_mostafa",
                Email = "mostafa.tech@posm-egypt.com",
                FullNameEn = "Mostafa Ibrahim",
                FullNameAr = "مصطفى إبراهيم",
                RoleId = technicianRole?.Id,
                Mobile = "+20 122 666 0006",
                PasswordHash = defaultHash,
                PasswordSalt = defaultSalt,
                PreferredLanguage = "ar",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                UserName = "tech_khaled",
                Email = "khaled.tech@posm-egypt.com",
                FullNameEn = "Khaled El-Shazly",
                FullNameAr = "خالد الشاذلي",
                RoleId = technicianRole?.Id,
                Mobile = "+20 122 777 0007",
                PasswordHash = defaultHash,
                PasswordSalt = defaultSalt,
                PreferredLanguage = "ar",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                UserName = "user",
                Email = "nouran.sales@posm-egypt.com",
                FullNameEn = "Nouran Mansour",
                FullNameAr = "نوران منصور",
                RoleId = userRole?.Id,
                Mobile = "+20 101 888 0008",
                PasswordHash = defaultHash,
                PasswordSalt = defaultSalt,
                PreferredLanguage = "en",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        foreach (var user in usersToSeed)
        {
            var existing = await context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            if (existing == null)
            {
                await context.Users.AddAsync(user);
            }
            else
            {
                existing.FullNameEn = user.FullNameEn;
                existing.FullNameAr = user.FullNameAr;
                existing.Email = user.Email;
                existing.Mobile = user.Mobile;
                existing.RoleId = user.RoleId;
                existing.PreferredLanguage = user.PreferredLanguage;
                existing.PasswordHash = defaultHash;
                existing.PasswordSalt = defaultSalt;
                existing.IsActive = true;
            }
        }

        await context.SaveChangesAsync();
    }

    private static (string Hash, string Salt) HashPasswordWithSalt(string password)
    {
        byte[] saltBytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(16);
        byte[] hashBytes = System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2(
            System.Text.Encoding.UTF8.GetBytes(password),
            saltBytes,
            iterations: 100000,
            hashAlgorithm: System.Security.Cryptography.HashAlgorithmName.SHA256,
            outputLength: 32);

        return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
    }

    private static async Task SeedCategoriesAsync(CleanSampleDbContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        var categories = new List<Category>
        {
            new Category
            {
                NameEn = "Floor Standing Displays (FSDU)",
                NameAr = "وحدات العرض الأرضية (FSDU)",
                DescriptionEn = "Freestanding multi-tier floor display units for retail stores and hypermarkets",
                DescriptionAr = "وحدات عرض أرضية متعددة الأرفف قائمة بذاتها للمتاجر والهايبر ماركت"
            },
            new Category
            {
                NameEn = "Countertop Displays (CDU)",
                NameAr = "وحدات عرض الكاونتر (CDU)",
                DescriptionEn = "Compact promotional counter display units for cash desk and impulse zones",
                DescriptionAr = "وحدات عرض مدمجة للكاونتر ومنطقة الكاشير للشراء الفوري"
            },
            new Category
            {
                NameEn = "Gondola End & Cladding",
                NameAr = "تكسية وتجهيز نهايات الجوندولا",
                DescriptionEn = "Branded end-cap cladding and headers for supermarket aisles",
                DescriptionAr = "تكسيات وهياكل دعائية مخصصة لنهايات ممرات السوبرماركت"
            },
            new Category
            {
                NameEn = "Lightboxes & Illuminated Signs",
                NameAr = "الصناديق المضيئة واللافتات (Lightbox)",
                DescriptionEn = "LED fabric SEG lightboxes and illuminated branded panels",
                DescriptionAr = "صناديق إضاءة LED قماشية بدون إطار ولافتات دعائية مضيئة"
            },
            new Category
            {
                NameEn = "Digital & Interactive Kiosks",
                NameAr = "الأكشاك والشاشات التفاعلية",
                DescriptionEn = "Interactive digital totems and commercial smart display stands",
                DescriptionAr = "شاشات عرض رقمية ذكية وأكشاك تفاعلية للأماكن التجارية"
            },
            new Category
            {
                NameEn = "Shelf Branding & POS Accessories",
                NameAr = "إكسسوارات وعلامات الأرفف",
                DescriptionEn = "Shelf-talkers, wobblers, price strips, and glorifiers",
                DescriptionAr = "أشرطة أسعار وبطاقات الرف المعلقة وحوامل العرض المميزة"
            }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedColorsAsync(CleanSampleDbContext context)
    {
        if (await context.Colors.AnyAsync()) return;

        var colors = new List<Color>
        {
            new Color { NameEn = "Retail Gloss Red", NameAr = "أحمر تجاري لامع", Code = "#E60000" },
            new Color { NameEn = "Corporate Royal Blue", NameAr = "أزرق ملكي", Code = "#0033A0" },
            new Color { NameEn = "Matte Black", NameAr = "أسود مطفي", Code = "#1A1A1A" },
            new Color { NameEn = "Pure White", NameAr = "أبيض ناصع", Code = "#FFFFFF" },
            new Color { NameEn = "Corporate Gold", NameAr = "ذهبي تجاري", Code = "#D4AF37" },
            new Color { NameEn = "Brushed Silver", NameAr = "فضي مصقول", Code = "#C0C0C0" }
        };

        await context.Colors.AddRangeAsync(colors);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMaterialsAsync(CleanSampleDbContext context)
    {
        if (await context.Materials.AnyAsync()) return;

        var materials = new List<Material>
        {
            new Material
            {
                NameEn = "High-Density Acrylic (Plexiglass)",
                NameAr = "أكريليك عالي الكثافة (بلكسي جلاس)",
                DescriptionEn = "Laser-cut high-transparency and colored cast acrylic sheets",
                DescriptionAr = "ألواح أكريليك مصبوبة مقطوعة بالليزر عالية الشفافية وملونة"
            },
            new Material
            {
                NameEn = "Forex PVC Foam Board",
                NameAr = "ألواح فوم فوركس PVC",
                DescriptionEn = "Lightweight high-rigidity expanded PVC foam sheets for UV printing",
                DescriptionAr = "ألواح بي في سي فوم خفيفة وصلبة مخصصة للطباعة الرقمية المباشرة"
            },
            new Material
            {
                NameEn = "Powder-Coated Steel",
                NameAr = "صلب مطلي بالبودرة الإلكتروستاتيكية",
                DescriptionEn = "Heavy-duty electrostatically powder-coated tubular steel and wire",
                DescriptionAr = "هياكل حديدية ومواسير صلب مطلية ببودرة إلكتروستاتيك مقاومة للخدش"
            },
            new Material
            {
                NameEn = "Anodized Aluminum Profile",
                NameAr = "قطاعات ألومنيوم مؤكسدة",
                DescriptionEn = "Extruded modular anodized aluminum profiles for SEG lightboxes and framing",
                DescriptionAr = "قطاعات ألومنيوم مسحوبة مؤكسدة مخصصة لإطارات الأقمشة واللافتات"
            },
            new Material
            {
                NameEn = "Laminated MDF Wood",
                NameAr = "خشب إم دي إف مصفح",
                DescriptionEn = "CNC-routed high-density MDF with melamine or wood grain laminate",
                DescriptionAr = "خشب إم دي إف عالي الكثافة مفرز بالكمبيوتر ومغطى بطبقة ميلامين"
            },
            new Material
            {
                NameEn = "Corrugated Fluted Board",
                NameAr = "كرتون مضلع مقوى",
                DescriptionEn = "Heavy-gauge B/EB flute corrugated board for temporary retail dump bins",
                DescriptionAr = "كرتون مضلع سميك مقوى مخصص لحاويات العروض الترويجية المؤقتة"
            }
        };

        await context.Materials.AddRangeAsync(materials);
        await context.SaveChangesAsync();
    }

    private static async Task SeedDesignsAsync(CleanSampleDbContext context)
    {
        if (await context.Designs.AnyAsync()) return;

        var designs = new List<Design>
        {
            new Design
            {
                NameEn = "Modular Knockdown (Flat-Pack)",
                NameAr = "نمطي قابل للفك والتركيب (تغليف مسطح)",
                DescriptionEn = "Easy tool-less on-site assembly designed for flat transportation",
                DescriptionAr = "تصميم نمطي يسهل تجميعه ميدانياً بدون أدوات وتوفير تكاليف الشحن"
            },
            new Design
            {
                NameEn = "Heavy-Duty Multi-Tier",
                NameAr = "متعدد الطبقات للأوزان الثقيلة",
                DescriptionEn = "Reinforced steel framing capable of holding up to 35kg per shelf",
                DescriptionAr = "هيكل صلب مقوى يتحمل حتى 35 كجم للرف الواحد في متاجر التجزئة"
            },
            new Design
            {
                NameEn = "Slimline Backlit SEG",
                NameAr = "نحيف بإضاءة خلفية وقماش سيليكون",
                DescriptionEn = "Ultra-thin 60mm profile with edge-lit LED modules and silicone edge fabric",
                DescriptionAr = "هيكل نحيف 60 مم بإضاءة ليد طرفية وشريط سيليكون لسهولة تغيير القماش"
            },
            new Design
            {
                NameEn = "Freestanding Rotating 360",
                NameAr = "ستاند دوار قائم 360 درجة",
                DescriptionEn = "Multi-sided rotating floor display with ball-bearing smooth rotation",
                DescriptionAr = "ستاند أرضي دوار متعدد الأوجه برولمان بلي لحركة دورانية ناعمة"
            },
            new Design
            {
                NameEn = "Curved Ergonomic Kiosk",
                NameAr = "كشك انسيابي مريح وسهل الاستخدام",
                DescriptionEn = "Aerodynamic curved pedestal designed for optimum customer eye-level viewing",
                DescriptionAr = "قاعدة انسيابية منحنية مصممة لزاوية رؤية مثالية ومريحة للعملاء"
            }
        };

        await context.Designs.AddRangeAsync(designs);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(CleanSampleDbContext context)
    {
        if (await context.Products.AnyAsync()) return;

        var categories = await context.Categories.ToListAsync();
        var colors = await context.Colors.ToListAsync();
        var materials = await context.Materials.ToListAsync();
        var designs = await context.Designs.ToListAsync();

        var fsduCategory = categories.FirstOrDefault(c => c.NameEn.Contains("FSDU")) ?? categories[0];
        var cduCategory = categories.FirstOrDefault(c => c.NameEn.Contains("CDU")) ?? categories[1];
        var gondolaCategory = categories.FirstOrDefault(c => c.NameEn.Contains("Gondola")) ?? categories[2];
        var lightboxCategory = categories.FirstOrDefault(c => c.NameEn.Contains("Lightbox")) ?? categories[3];
        var digitalCategory = categories.FirstOrDefault(c => c.NameEn.Contains("Digital")) ?? categories[4];
        var shelfCategory = categories.FirstOrDefault(c => c.NameEn.Contains("Shelf")) ?? categories[5];

        var redColor = colors.FirstOrDefault(c => c.Code == "#E60000") ?? colors[0];
        var blueColor = colors.FirstOrDefault(c => c.Code == "#0033A0") ?? colors[1];
        var blackColor = colors.FirstOrDefault(c => c.Code == "#1A1A1A") ?? colors[2];
        var whiteColor = colors.FirstOrDefault(c => c.Code == "#FFFFFF") ?? colors[3];

        var steelMat = materials.FirstOrDefault(m => m.NameEn.Contains("Steel")) ?? materials[0];
        var acrylicMat = materials.FirstOrDefault(m => m.NameEn.Contains("Acrylic")) ?? materials[0];
        var alumMat = materials.FirstOrDefault(m => m.NameEn.Contains("Aluminum")) ?? materials[0];
        var mdfMat = materials.FirstOrDefault(m => m.NameEn.Contains("MDF")) ?? materials[0];

        var knockdownDesign = designs.FirstOrDefault(d => d.NameEn.Contains("Knockdown")) ?? designs[0];
        var heavyDesign = designs.FirstOrDefault(d => d.NameEn.Contains("Heavy-Duty")) ?? designs[0];
        var slimDesign = designs.FirstOrDefault(d => d.NameEn.Contains("Slimline")) ?? designs[0];
        var rotatingDesign = designs.FirstOrDefault(d => d.NameEn.Contains("Rotating")) ?? designs[0];
        var curvedDesign = designs.FirstOrDefault(d => d.NameEn.Contains("Curved")) ?? designs[0];

        var products = new List<Product>
        {
            new Product
            {
                CategoryId = fsduCategory.Id,
                ColorId = redColor.Id,
                MaterialId = steelMat.Id,
                DesignId = heavyDesign.Id,
                NameEn = "FSDU 4-Tier Heavy-Duty Floor Display",
                NameAr = "وحدة عرض أرضية 4 أرفف للمهام الشاقة (FSDU)",
                Barcode = "622100010001",
                DescriptionEn = "Premium 4-tier freestanding floor display unit built with powder-coated steel frame, branded Forex side panels, and illuminated top header for FMCG hypermarket activations.",
                DescriptionAr = "وحدة عرض أرضية فاخرة 4 أرفف قائمة بذاتها بهيكل صلب مطلي ببودرة وألواح فوركس جانبية دعائية وهيدر مضيء لتنشيط المبيعات بالهايبر ماركت.",
                PictureUrl = "/uploads/products/fsdu-4tier-display.jpg",
                IsActive = true
            },
            new Product
            {
                CategoryId = cduCategory.Id,
                ColorId = whiteColor.Id,
                MaterialId = acrylicMat.Id,
                DesignId = knockdownDesign.Id,
                NameEn = "Acrylic Countertop Display Unit (CDU) 3-Shelf",
                NameAr = "وحدة عرض كاونتر من الأكريليك 3 أرفف (CDU)",
                Barcode = "622100010002",
                DescriptionEn = "Clear 3-shelf acrylic counter display with branded exchangeable front header and modular tier dividers for checkout lanes and pharmacy counters.",
                DescriptionAr = "وحدة عرض كاونتر شفافة 3 أرفف من الأكريليك الفاخر مع هيدر أمامي قابل للتغيير وفواصل نمطية لمنطقة الكاشير وصيدليات التجزئة.",
                PictureUrl = "/uploads/products/acrylic-cdu-3tier.jpg",
                IsActive = true
            },
            new Product
            {
                CategoryId = gondolaCategory.Id,
                ColorId = blueColor.Id,
                MaterialId = steelMat.Id,
                DesignId = knockdownDesign.Id,
                NameEn = "Gondola End-Cap Promotional Cladding System",
                NameAr = "نظام تجليد وتجهيز نهايات ممرات الجوندولا الترويجي",
                Barcode = "622100010003",
                DescriptionEn = "Complete supermarket gondola end-cap framing with LED backlit graphic panels, reinforced shelf covers, and side channel branding.",
                DescriptionAr = "هيكل متكامل لتجليد وتجهيز نهاية ممرات السوبرماركت بألواح إعلانية مضيئة LED وتكسيات أرفف مقواة وعلامات جانبية بارزة.",
                PictureUrl = "/uploads/products/gondola-endcap-cladding.jpg",
                IsActive = true
            },
            new Product
            {
                CategoryId = lightboxCategory.Id,
                ColorId = blackColor.Id,
                MaterialId = alumMat.Id,
                DesignId = slimDesign.Id,
                NameEn = "Frameless Fabric LED Lightbox (SEG 100x200cm)",
                NameAr = "صندوق إضاءة قماشي بدون إطار (SEG مقاس 100×200 سم)",
                Barcode = "622100010004",
                DescriptionEn = "Ultra-slim 60mm aluminum SEG textile lightbox with high-output 12V LED modules and quick-fit silicone edge graphic fabric.",
                DescriptionAr = "صندوق إضاءة إعلاني ألومنيوم نحيف 60 مم ذو قماش مطبوع بإطار سيليكون مخفي وإضاءة LED عالية الكفاءة 12 فولت.",
                PictureUrl = "/uploads/products/frameless-seg-lightbox.jpg",
                IsActive = true
            },
            new Product
            {
                CategoryId = digitalCategory.Id,
                ColorId = blackColor.Id,
                MaterialId = steelMat.Id,
                DesignId = curvedDesign.Id,
                NameEn = "Interactive Touchscreen Digital Kiosk 43-Inch",
                NameAr = "كشك رقمي تفاعلي بشاشة لمس 43 بوصة",
                Barcode = "622100010005",
                DescriptionEn = "Freestanding 43-inch interactive marketing totem with commercial 4K panel, integrated Android player, and heavy tempered glass facade.",
                DescriptionAr = "شاشة تفاعلية دعائية قائمة بذاتها 43 بوصة بدقة 4K ومشغل أندرويد مدمج مع واجهة زجاجية مقواة ومقاومة للصدمات.",
                PictureUrl = "/uploads/products/digital-kiosk-43.jpg",
                IsActive = true
            },
            new Product
            {
                CategoryId = shelfCategory.Id,
                ColorId = redColor.Id,
                MaterialId = materials.FirstOrDefault(m => m.NameEn.Contains("Corrugated"))?.Id ?? mdfMat.Id,
                DesignId = knockdownDesign.Id,
                NameEn = "Promotional Dump Bin Hexagonal Core",
                NameAr = "حاوية عروض ترويجية سداسية (دمب بن)",
                Barcode = "622100010006",
                DescriptionEn = "High-capacity retail dump bin with adjustable reinforced false bottom and scratch-resistant gloss laminated brand graphics.",
                DescriptionAr = "حاوية عروض ترويجية سداسية سعة كبيرة للأصناف المخفضة مع قاعدة داخلية قابلة لتعديل الارتفاع وطباعة مصفحة لامعة.",
                PictureUrl = "/uploads/products/dump-bin-hexagonal.jpg",
                IsActive = true
            },
            new Product
            {
                CategoryId = shelfCategory.Id,
                ColorId = whiteColor.Id,
                MaterialId = acrylicMat.Id,
                DesignId = slimDesign.Id,
                NameEn = "Plexiglass Cosmetic Glorifier Stand",
                NameAr = "حامل مستحضرات تجميل فاخر من الأكريليك (جلوريفاير)",
                Barcode = "622100010007",
                DescriptionEn = "Luxury illuminated cosmetic glorifier with magnetic product cutouts, halo warm white LED lighting, and polished crystal edges.",
                DescriptionAr = "حامل عرض مستحضرات تجميل مضيء فاخر مع قوالب تثبيت مغناطيسية وإضاءة LED بيضاء دافئة وحواف كريستالية مصقولة.",
                PictureUrl = "/uploads/products/cosmetic-glorifier-stand.jpg",
                IsActive = true
            },
            new Product
            {
                CategoryId = fsduCategory.Id,
                ColorId = blueColor.Id,
                MaterialId = steelMat.Id,
                DesignId = rotatingDesign.Id,
                NameEn = "Rotating 360 Floor Wire & Forex Spinner Stand",
                NameAr = "ستاند دوار 360 درجة سلكي وفوركس للأرضيات",
                Barcode = "622100010008",
                DescriptionEn = "Four-sided rotating floor spinner stand equipped with 24 euro-hooks, header topper card, and smooth industrial ball bearings.",
                DescriptionAr = "ستاند دوار 4 أوجه مزود بـ 24 خطاف تعليق أوروبي وهيدر علوي دائري ورولمان بلي صناعي لحركة سلسة.",
                PictureUrl = "/uploads/products/rotating-spinner-stand.jpg",
                IsActive = true
            },
            new Product
            {
                CategoryId = shelfCategory.Id,
                ColorId = whiteColor.Id,
                MaterialId = acrylicMat.Id,
                DesignId = knockdownDesign.Id,
                NameEn = "Magnetic Shelf-Talker & Price Rail Profile Kit",
                NameAr = "طقم مسار أسعار مغناطيسي مع بطاقة تحدث للرف",
                Barcode = "622100010009",
                DescriptionEn = "Extruded clear PVC shelf-edge rail with strong magnetic tape backing and flexible wobbler gripper arms for shelf branding.",
                DescriptionAr = "مجرى أسعار شفاف من مادة PVC مزود بشريط مغناطيسي خلفي قوي ومقابض مرنة لتثبيت لافتات الرف المتحركة (الووبلر).",
                PictureUrl = "/uploads/products/magnetic-shelf-talker.jpg",
                IsActive = true
            },
            new Product
            {
                CategoryId = cduCategory.Id,
                ColorId = blackColor.Id,
                MaterialId = mdfMat.Id,
                DesignId = heavyDesign.Id,
                NameEn = "Premium Laminated MDF Counter Showcase with Lock",
                NameAr = "كاونتر عرض فاخر من خشب MDF بقفل أمان",
                Barcode = "622100010010",
                DescriptionEn = "Secure retail counter showcase made with laminated black MDF, tempered glass cover, internal LED spotlight, and key lock.",
                DescriptionAr = "فترينة كاونتر عرض آمنة مصنوعة من خشب MDF المصفح مع زجاج مقوى وإضاءة سبوت لايت وقفل أمان للمنتجات القيمة.",
                PictureUrl = "/uploads/products/mdf-counter-showcase.jpg",
                IsActive = true
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPartsAsync(CleanSampleDbContext context)
    {
        if (await context.Parts.AnyAsync()) return;

        var parts = new List<Part>
        {
            new Part
            {
                Code = "PART-STL-BASE-01",
                NameEn = "Heavy Steel Weighted Base Plate",
                NameAr = "قاعدة صلب ثقيلة الوزن للاتزان",
                DescriptionEn = "5mm laser-cut heavy steel floor stabilizer base plate with matte black powder coating",
                DescriptionAr = "لوح صلب مقطوع بالليزر سمك 5 مم لقاعدة الاتزان الأرضية مطلي ببودرة إلكتروستاتيك سوداء",
                Barcode = "622200010001",
                IsActive = true
            },
            new Part
            {
                Code = "PART-ACR-SHLF-02",
                NameEn = "Clear Acrylic Shelf 4mm Diamond Polished",
                NameAr = "رف أكريليك شفاف 4 مم بحواف مصقولة",
                DescriptionEn = "Cast transparent acrylic shelf with CNC rounded corners and diamond flame-polished edges",
                DescriptionAr = "رف أكريليك مصبوب فائق الشفافية مع زوايا دائرية وحواف مصقولة بلهب الماس",
                Barcode = "622200010002",
                IsActive = true
            },
            new Part
            {
                Code = "PART-FRX-SIDE-03",
                NameEn = "Forex PVC 5mm UV Printed Side Panel",
                NameAr = "لوح جانبي فوركس 5 مم مطبوع UV",
                DescriptionEn = "High-density 5mm Forex PVC side panel with double-sided photographic direct UV print",
                DescriptionAr = "لوح فوركس 5 مم عالي الكثافة مع طباعة رقمية مباشرة UV ملونة على الوجهين",
                Barcode = "622200010003",
                IsActive = true
            },
            new Part
            {
                Code = "PART-LED-MOD-04",
                NameEn = "High-Lumen 12V LED Strip Module (6500K)",
                NameAr = "وحدة شريط ليد 12 فولت عالي السطوع (6500 كلفن)",
                DescriptionEn = "Samsung chip rigid LED backlighting strip module with aluminum heat-sink backing",
                DescriptionAr = "شريط إضاءة ليد صلب بشرائح سامسونج وقاعدة ألومنيوم لتشتيت الحرارة",
                Barcode = "622200010004",
                IsActive = true
            },
            new Part
            {
                Code = "PART-PWR-SUP-05",
                NameEn = "MeanWell 100W 12V DC Slim Power Supply",
                NameAr = "محول طاقة نحيف مين ويل 100 واط 12 فولت",
                DescriptionEn = "Industrial grade MeanWell AC-DC switching LED driver with short-circuit protection",
                DescriptionAr = "محول طاقة صناعي ماركة مين ويل مزود بحماية ضد قصر الدائرة الكهربائية",
                Barcode = "622200010005",
                IsActive = true
            },
            new Part
            {
                Code = "PART-ALM-SEG-06",
                NameEn = "Anodized Aluminum 60mm SEG Lightbox Profile",
                NameAr = "قطاع ألومنيوم 60 مم مؤكسد لصناديق SEG",
                DescriptionEn = "Extruded 60mm depth aluminum channel profile engineered for silicone edge textile insertion",
                DescriptionAr = "مجرى ألومنيوم مسحوب عمق 60 مم مصمم خصيصاً لتثبيت حواف قماش السيليكون",
                Barcode = "622200010006",
                IsActive = true
            },
            new Part
            {
                Code = "PART-HDR-ACR-07",
                NameEn = "Illuminated Acrylic Brand Header Topper",
                NameAr = "هيدر علوي أكريليك دعائي مضيء",
                DescriptionEn = "Thermoformed acrylic header unit with 3D embossed logo cutout and internal diffuser",
                DescriptionAr = "هيدر أكريليك مشكل حرارياً مع شعار ثلاثي الأبعاد بارز ومشتت إضاءة داخلي",
                Barcode = "622200010007",
                IsActive = true
            },
            new Part
            {
                Code = "PART-HOK-EUR-08",
                NameEn = "Zinc-Plated 20cm Double Wire Euro-Hook",
                NameAr = "خطاف تعليق أوروبي مزدوج مجلفن 20 سم",
                DescriptionEn = "Heavy-duty 6mm steel wire euro-hook with zinc plating and price tag flip holder",
                DescriptionAr = "خطاف تعليق سلكي صلب 6 مم مجلفن بطول 20 سم مع حامل لبطاقة السعر القلابة",
                Barcode = "622200010008",
                IsActive = true
            },
            new Part
            {
                Code = "PART-MDF-BAS-09",
                NameEn = "CNC Routed 18mm Melamine MDF Base Box",
                NameAr = "صندوق قاعدة خشب MDF ملامين 18 مم مفرز بالكمبيوتر",
                DescriptionEn = "Precision CNC routed moisture-resistant MDF base pedestal with black melamine finish",
                DescriptionAr = "قاعدة خشب MDF مقاوم للرطوبة 18 مم مفرز بدقة CNC ومغطى بطبقة ملامين سوداء",
                Barcode = "622200010009",
                IsActive = true
            },
            new Part
            {
                Code = "PART-DISP-43-10",
                NameEn = "43-Inch 4K Commercial Touch Display Panel",
                NameAr = "شاشة عرض تجارية 43 بوصة بدقة 4K مع خاصية اللمس",
                DescriptionEn = "Capacitive 10-point touch commercial grade panel with 500 nits brightness and 24/7 duty cycle",
                DescriptionAr = "شاشة تجارية سريعة الاستجابة تعمل باللمس بقدرة تشغيل 24/7 وسطوع 500 شمعة",
                Barcode = "622200010010",
                IsActive = true
            },
            new Part
            {
                Code = "PART-MED-BOX-11",
                NameEn = "Android 4K Commercial Signage Media Player",
                NameAr = "مشغل وسائط رقمية تجاري 4K بنظام أندرويد",
                DescriptionEn = "Quad-core Android media box with auto-reboot, scheduled playback, and HDMI 2.0 output",
                DescriptionAr = "جهاز تشغيل وسائط رقمي أندرويد رباعي النواة مع تشغيل مجدول وإعادة تشغيل تلقائية",
                Barcode = "622200010011",
                IsActive = true
            },
            new Part
            {
                Code = "PART-DIV-ACR-12",
                NameEn = "Molded Acrylic Shelf Divider with Pushers",
                NameAr = "فاصل أرفف أكريليك مقولب مع دافع منتجات زنبركي",
                DescriptionEn = "Crystal clear molded acrylic shelf divider equipped with automatic spring-loaded product pusher",
                DescriptionAr = "فاصل رف أكريليك شفاف مقولب مزود بدافع أوتوماتيكي زنبركي للمنتجات",
                Barcode = "622200010012",
                IsActive = true
            },
            new Part
            {
                Code = "PART-MAG-STR-13",
                NameEn = "Neodymium Magnetic Mounting Base Strip 1m",
                NameAr = "شريط تثبيت مغناطيسي نيوديميوم قوي بطول 1 متر",
                DescriptionEn = "Heavy magnetic extruded flexible strip with 3M VHB self-adhesive backing",
                DescriptionAr = "شريط مغناطيسي مرن وقوي مدعوم بلاصق 3M VHB فائق الالتصاق",
                Barcode = "622200010013",
                IsActive = true
            },
            new Part
            {
                Code = "PART-PVC-RAIL-14",
                NameEn = "Extruded Clear PVC Price Strip Rail 100cm",
                NameAr = "مجرى أسعار شفاف PVC بطول 100 سم",
                DescriptionEn = "UV-stabilized clear front PVC price profile with snap-fit angle adjustment",
                DescriptionAr = "مجرى بلاستيك شفاف مقاوم للأشعة فوق البنفسجية لبطاقات الأسعار مع تعديل زاوية الرؤية",
                Barcode = "622200010014",
                IsActive = true
            },
            new Part
            {
                Code = "PART-CRG-HEX-15",
                NameEn = "Die-Cut Heavy Fluted Corrugated Body Outer",
                NameAr = "جسم خارجي كرتون مضلع مقوى ومقطع بدقة بالداي كت",
                DescriptionEn = "EB-flute double-wall corrugated dump bin body with waterproof bottom coating",
                DescriptionAr = "جسم كرتون مضلع مزدوج الجدار لحاويات العروض مع طبقة سفلية مقاومة للماء",
                Barcode = "622200010015",
                IsActive = true
            },
            new Part
            {
                Code = "PART-WHL-CST-16",
                NameEn = "Industrial Swivel Rubber Casters with Brake (Set of 4)",
                NameAr = "طقم 4 عجلات مطاطية دوارة شديدة التحمل بمكابح",
                DescriptionEn = "75mm heavy-duty ball bearing polyurethane wheels with step-on locking brakes",
                DescriptionAr = "عجلات بولي يوريثان 75 مم شديدة التحمل برولمان بلي مع فرامل تثبيت قدم",
                Barcode = "622200010016",
                IsActive = true
            },
            new Part
            {
                Code = "PART-FAB-SEG-17",
                NameEn = "Dye-Sublimated Backlit Stretch Textile Fabric",
                NameAr = "قماش مطاطي مطبوع تسامي مخصص للإضاءة الخلفية",
                DescriptionEn = "Wrinkle-resistant Samba backlit fabric with stitched 14x3mm silicone keder edge",
                DescriptionAr = "قماش سامبا مطبوع حرارياً مقاوم للتجعد مع حبل سيليكون 14×3 مم مخيط على الأطراف",
                Barcode = "622200010017",
                IsActive = true
            },
            new Part
            {
                Code = "PART-CAM-LCK-18",
                NameEn = "Quick-Assembly Eccentric Cam Locks & Dowels (Bag of 20)",
                NameAr = "كيس 20 طقم كوامات وتثبيت سريع للتجميع الميداني",
                DescriptionEn = "Zinc alloy eccentric cam connectors with steel dowels for rapid knockdown booth assembly",
                DescriptionAr = "مسامير وكوامات سبيكة زنك وصلب لتثبيت الهياكل الخشبية والمعدنية بسرعة في الموقع",
                Barcode = "622200010018",
                IsActive = true
            }
        };

        await context.Parts.AddRangeAsync(parts);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductBOMAsync(CleanSampleDbContext context)
    {
        if (await context.ProductBOMs.AnyAsync()) return;

        var products = await context.Products.ToListAsync();
        var parts = await context.Parts.ToDictionaryAsync(p => p.Code);

        var boms = new List<ProductBOM>();

        // 1. FSDU 4-Tier Heavy-Duty Floor Display
        var fsdu = products.FirstOrDefault(p => p.Barcode == "622100010001");
        if (fsdu != null)
        {
            if (parts.TryGetValue("PART-STL-BASE-01", out var basePlate))
                boms.Add(new ProductBOM { ProductId = fsdu.Id, PartId = basePlate.Id, Quantity = 1.0m });
            if (parts.TryGetValue("PART-FRX-SIDE-03", out var sidePanels))
                boms.Add(new ProductBOM { ProductId = fsdu.Id, PartId = sidePanels.Id, Quantity = 2.0m });
            if (parts.TryGetValue("PART-HDR-ACR-07", out var headerSign))
                boms.Add(new ProductBOM { ProductId = fsdu.Id, PartId = headerSign.Id, Quantity = 1.0m });
            if (parts.TryGetValue("PART-PVC-RAIL-14", out var priceRails))
                boms.Add(new ProductBOM { ProductId = fsdu.Id, PartId = priceRails.Id, Quantity = 4.0m });
            if (parts.TryGetValue("PART-CAM-LCK-18", out var camLocks))
                boms.Add(new ProductBOM { ProductId = fsdu.Id, PartId = camLocks.Id, Quantity = 1.0m });
        }

        // 2. Acrylic Countertop Display Unit (CDU) 3-Shelf
        var cdu = products.FirstOrDefault(p => p.Barcode == "622100010002");
        if (cdu != null)
        {
            if (parts.TryGetValue("PART-ACR-SHLF-02", out var shelves))
                boms.Add(new ProductBOM { ProductId = cdu.Id, PartId = shelves.Id, Quantity = 3.0m });
            if (parts.TryGetValue("PART-DIV-ACR-12", out var dividers))
                boms.Add(new ProductBOM { ProductId = cdu.Id, PartId = dividers.Id, Quantity = 6.0m });
            if (parts.TryGetValue("PART-HDR-ACR-07", out var cduHeader))
                boms.Add(new ProductBOM { ProductId = cdu.Id, PartId = cduHeader.Id, Quantity = 1.0m });
        }

        // 3. Gondola End-Cap Promotional Cladding System
        var gondola = products.FirstOrDefault(p => p.Barcode == "622100010003");
        if (gondola != null)
        {
            if (parts.TryGetValue("PART-ALM-SEG-06", out var almFrame))
                boms.Add(new ProductBOM { ProductId = gondola.Id, PartId = almFrame.Id, Quantity = 4.0m });
            if (parts.TryGetValue("PART-LED-MOD-04", out var ledMod))
                boms.Add(new ProductBOM { ProductId = gondola.Id, PartId = ledMod.Id, Quantity = 6.0m });
            if (parts.TryGetValue("PART-PWR-SUP-05", out var pwrSupply))
                boms.Add(new ProductBOM { ProductId = gondola.Id, PartId = pwrSupply.Id, Quantity = 1.0m });
            if (parts.TryGetValue("PART-FRX-SIDE-03", out var sideBanners))
                boms.Add(new ProductBOM { ProductId = gondola.Id, PartId = sideBanners.Id, Quantity = 2.0m });
        }

        // 4. Frameless Fabric LED Lightbox (SEG 100x200cm)
        var lightbox = products.FirstOrDefault(p => p.Barcode == "622100010004");
        if (lightbox != null)
        {
            if (parts.TryGetValue("PART-ALM-SEG-06", out var segFrame))
                boms.Add(new ProductBOM { ProductId = lightbox.Id, PartId = segFrame.Id, Quantity = 4.0m });
            if (parts.TryGetValue("PART-FAB-SEG-17", out var segFabric))
                boms.Add(new ProductBOM { ProductId = lightbox.Id, PartId = segFabric.Id, Quantity = 2.0m });
            if (parts.TryGetValue("PART-LED-MOD-04", out var segLeds))
                boms.Add(new ProductBOM { ProductId = lightbox.Id, PartId = segLeds.Id, Quantity = 8.0m });
            if (parts.TryGetValue("PART-PWR-SUP-05", out var segPower))
                boms.Add(new ProductBOM { ProductId = lightbox.Id, PartId = segPower.Id, Quantity = 1.0m });
        }

        // 5. Interactive Touchscreen Digital Kiosk 43-Inch
        var kiosk = products.FirstOrDefault(p => p.Barcode == "622100010005");
        if (kiosk != null)
        {
            if (parts.TryGetValue("PART-DISP-43-10", out var screen))
                boms.Add(new ProductBOM { ProductId = kiosk.Id, PartId = screen.Id, Quantity = 1.0m });
            if (parts.TryGetValue("PART-MED-BOX-11", out var mediaBox))
                boms.Add(new ProductBOM { ProductId = kiosk.Id, PartId = mediaBox.Id, Quantity = 1.0m });
            if (parts.TryGetValue("PART-STL-BASE-01", out var kioskBase))
                boms.Add(new ProductBOM { ProductId = kiosk.Id, PartId = kioskBase.Id, Quantity = 1.0m });
            if (parts.TryGetValue("PART-PWR-SUP-05", out var kioskPwr))
                boms.Add(new ProductBOM { ProductId = kiosk.Id, PartId = kioskPwr.Id, Quantity = 1.0m });
        }

        // 6. Promotional Dump Bin Hexagonal Core
        var dumpBin = products.FirstOrDefault(p => p.Barcode == "622100010006");
        if (dumpBin != null)
        {
            if (parts.TryGetValue("PART-CRG-HEX-15", out var binCore))
                boms.Add(new ProductBOM { ProductId = dumpBin.Id, PartId = binCore.Id, Quantity = 1.0m });
            if (parts.TryGetValue("PART-HDR-ACR-07", out var binHeader))
                boms.Add(new ProductBOM { ProductId = dumpBin.Id, PartId = binHeader.Id, Quantity = 1.0m });
        }

        // 7. Plexiglass Cosmetic Glorifier Stand
        var glorifier = products.FirstOrDefault(p => p.Barcode == "622100010007");
        if (glorifier != null)
        {
            if (parts.TryGetValue("PART-ACR-SHLF-02", out var glorShlf))
                boms.Add(new ProductBOM { ProductId = glorifier.Id, PartId = glorShlf.Id, Quantity = 2.0m });
            if (parts.TryGetValue("PART-LED-MOD-04", out var glorLed))
                boms.Add(new ProductBOM { ProductId = glorifier.Id, PartId = glorLed.Id, Quantity = 2.0m });
            if (parts.TryGetValue("PART-PWR-SUP-05", out var glorPwr))
                boms.Add(new ProductBOM { ProductId = glorifier.Id, PartId = glorPwr.Id, Quantity = 1.0m });
        }

        // 8. Rotating 360 Floor Wire & Forex Spinner Stand
        var spinner = products.FirstOrDefault(p => p.Barcode == "622100010008");
        if (spinner != null)
        {
            if (parts.TryGetValue("PART-STL-BASE-01", out var spinBase))
                boms.Add(new ProductBOM { ProductId = spinner.Id, PartId = spinBase.Id, Quantity = 1.0m });
            if (parts.TryGetValue("PART-HOK-EUR-08", out var spinHooks))
                boms.Add(new ProductBOM { ProductId = spinner.Id, PartId = spinHooks.Id, Quantity = 24.0m });
            if (parts.TryGetValue("PART-FRX-SIDE-03", out var spinForex))
                boms.Add(new ProductBOM { ProductId = spinner.Id, PartId = spinForex.Id, Quantity = 4.0m });
            if (parts.TryGetValue("PART-WHL-CST-16", out var spinWheels))
                boms.Add(new ProductBOM { ProductId = spinner.Id, PartId = spinWheels.Id, Quantity = 1.0m });
        }

        if (boms.Any())
        {
            await context.ProductBOMs.AddRangeAsync(boms);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedVehiclesAsync(CleanSampleDbContext context)
    {
        if (await context.Vehicles.AnyAsync()) return;

        var vehicles = new List<Vehicle>
        {
            new Vehicle
            {
                VehicleNumber = "VEH-EGY-01",
                PlateNumber = "ط ر د ٥٢٩١",
                VehicleType = "Chevrolet Jumbo Box Truck (شيفورليه جامبو صندوق)",
                CapacityKg = 4500.00m,
                IsActive = true
            },
            new Vehicle
            {
                VehicleNumber = "VEH-EGY-02",
                PlateNumber = "أ ب ج ٨١٤٧",
                VehicleType = "Isuzu D-Max Crew Cab Pickup (إيسوزو ديماكس نقل)",
                CapacityKg = 1800.00m,
                IsActive = true
            },
            new Vehicle
            {
                VehicleNumber = "VEH-EGY-03",
                PlateNumber = "س ن ق ٦٣٢٤",
                VehicleType = "Mercedes-Benz Actros Heavy Freight (مرسيدس أكتروس مقطورة)",
                CapacityKg = 12000.00m,
                IsActive = true
            },
            new Vehicle
            {
                VehicleNumber = "VEH-EGY-04",
                PlateNumber = "م و هـ ١٩٥٢",
                VehicleType = "Toyota HiAce High Roof Cargo Van (تويوتا هاي إيس فان مغلق)",
                CapacityKg = 2200.00m,
                IsActive = true
            },
            new Vehicle
            {
                VehicleNumber = "VEH-EGY-05",
                PlateNumber = "ق د ص ٤٧١٣",
                VehicleType = "Hyundai Mighty Closed Box Truck (هيونداي مايتي شاسيه طويل)",
                CapacityKg = 3800.00m,
                IsActive = true
            }
        };

        await context.Vehicles.AddRangeAsync(vehicles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedClientsAsync(CleanSampleDbContext context)
    {
        if (await context.Clients.AnyAsync()) return;

        var clients = new List<Client>
        {
            new Client
            {
                Name = "Carrefour Egypt (Majid Al Futtaim)",
                Phone = "+20 2 16061",
                Mobile = "+20 100 123 4567",
                Email = "procurement.eg@carrefour.com",
                Address = "City Centre Almaza, Sheraton, Heliopolis",
                City = "Cairo",
                IsActive = true
            },
            new Client
            {
                Name = "Hyperone Egypt (هايبر وان مصر)",
                Phone = "+20 2 3850 0000",
                Mobile = "+20 111 234 5678",
                Email = "merchandising@hyperone.com.eg",
                Address = "El Mehwar El Markazi, Sheikh Zayed City",
                City = "Giza",
                IsActive = true
            },
            new Client
            {
                Name = "Spinneys Egypt (سبينيس مصر)",
                Phone = "+20 2 15355",
                Mobile = "+20 122 345 6789",
                Email = "posm.ops@spinneys-egypt.com",
                Address = "Mall of Arabia, 6th of October City",
                City = "Giza",
                IsActive = true
            },
            new Client
            {
                Name = "El Ezaby Pharmacies (مجموعة صيدليات العزبي)",
                Phone = "+20 2 19600",
                Mobile = "+20 101 999 8881",
                Email = "trade.marketing@elezaby.com",
                Address = "6 El-Bostan St, Bab El-Louk, Downtown",
                City = "Cairo",
                IsActive = true
            },
            new Client
            {
                Name = "B.TECH Egypt (شركة بي تك لتجارة التجزئة)",
                Phone = "+20 2 19966",
                Mobile = "+20 115 888 7772",
                Email = "visual.merchandising@btech.com",
                Address = "Building 24, Road 90 South, 5th Settlement, New Cairo",
                City = "Cairo",
                IsActive = true
            },
            new Client
            {
                Name = "Metro & Fresh Food Market (مترو ماركت)",
                Phone = "+20 2 19619",
                Mobile = "+20 120 777 6663",
                Email = "storedisplay@metro-markets.com",
                Address = "9 Damascus St, Roxy, Heliopolis",
                City = "Cairo",
                IsActive = true
            },
            new Client
            {
                Name = "Seif Pharmacies (مجموعة صيدليات سيف)",
                Phone = "+20 2 19199",
                Mobile = "+20 106 444 3335",
                Email = "procurement@seif-pharmacies.com",
                Address = "26 Mossadak St, Dokki",
                City = "Giza",
                IsActive = true
            }
        };

        await context.Clients.AddRangeAsync(clients);
        await context.SaveChangesAsync();
    }

    private static async Task SeedClientLocationsAsync(CleanSampleDbContext context)
    {
        if (await context.ClientLocations.AnyAsync()) return;

        var clients = await context.Clients.ToListAsync();
        if (!clients.Any()) return;

        var locations = new List<ClientLocation>();

        // Carrefour Locations
        var carrefour = clients.FirstOrDefault(c => c.Name.Contains("Carrefour"));
        if (carrefour != null)
        {
            locations.Add(new ClientLocation
            {
                ClientId = carrefour.Id,
                Name = "Carrefour - City Centre Almaza Branch (سيتي سنتر ألماظة)",
                Address = "Intersection of Suez Road & El-Nasr Rd, Sheraton, Heliopolis",
                City = "Cairo",
                ContactName = "Eng. Hazem El-Kordy (Store Operations)",
                ContactPhone = "+20 100 123 4567",
                Latitude = 30.0890m,
                Longitude = 31.3650m,
                IsDefault = true,
                IsActive = true
            });
            locations.Add(new ClientLocation
            {
                ClientId = carrefour.Id,
                Name = "Carrefour - City Centre Maadi Mega Branch (سيتي سنتر المعادي)",
                Address = "Ring Road, El-Basatin, Maadi",
                City = "Cairo",
                ContactName = "Mohamed Rashad (Merchandising Lead)",
                ContactPhone = "+20 100 123 9999",
                Latitude = 29.9720m,
                Longitude = 31.3040m,
                IsDefault = false,
                IsActive = true
            });
            locations.Add(new ClientLocation
            {
                ClientId = carrefour.Id,
                Name = "Carrefour - Mirage Mall New Cairo (فرع ميراج مول التجمع الأول)",
                Address = "Mirage Mall, Ring Road, 1st Settlement, New Cairo",
                City = "Cairo",
                ContactName = "Hossam Gabr (Receiving Supervisor)",
                ContactPhone = "+20 100 123 8888",
                Latitude = 30.0650m,
                Longitude = 31.4550m,
                IsDefault = false,
                IsActive = true
            });
        }

        // Hyperone Locations
        var hyperone = clients.FirstOrDefault(c => c.Name.Contains("Hyperone"));
        if (hyperone != null)
        {
            locations.Add(new ClientLocation
            {
                ClientId = hyperone.Id,
                Name = "Hyperone - Sheikh Zayed Flagship Branch (فرع الشيخ زايد الرئيسي)",
                Address = "Entrance 1, 26th of July Corridor, Sheikh Zayed City",
                City = "Giza",
                ContactName = "Tarek El-Dessouky (Display Supervisor)",
                ContactPhone = "+20 111 234 5678",
                Latitude = 30.0380m,
                Longitude = 30.9850m,
                IsDefault = true,
                IsActive = true
            });
            locations.Add(new ClientLocation
            {
                ClientId = hyperone.Id,
                Name = "Hyperone - Sphinx Airport Branch (فرع مطار سفنكس طريق الإسكندرية)",
                Address = "Km 55, Cairo - Alexandria Desert Road, Giza",
                City = "Giza",
                ContactName = "Sameh Nabil (Warehouse Manager)",
                ContactPhone = "+20 111 234 9999",
                Latitude = 30.1200m,
                Longitude = 30.8200m,
                IsDefault = false,
                IsActive = true
            });
        }

        // Spinneys Locations
        var spinneys = clients.FirstOrDefault(c => c.Name.Contains("Spinneys"));
        if (spinneys != null)
        {
            locations.Add(new ClientLocation
            {
                ClientId = spinneys.Id,
                Name = "Spinneys - Mall of Arabia (مول العرب 6 أكتوبر)",
                Address = "Juhayna Square, Mall of Arabia Gate 1, 6th of October City",
                City = "Giza",
                ContactName = "Amr Shawky (Floor Operations)",
                ContactPhone = "+20 122 345 6789",
                Latitude = 30.0070m,
                Longitude = 30.9730m,
                IsDefault = true,
                IsActive = true
            });
            locations.Add(new ClientLocation
            {
                ClientId = spinneys.Id,
                Name = "Spinneys - The Arena Mall New Cairo (فرع الأرينا التجمع الخامس)",
                Address = "North 90th Street, 5th Settlement, New Cairo",
                City = "Cairo",
                ContactName = "Wael Mansour (Visual Display Mgr)",
                ContactPhone = "+20 122 345 9999",
                Latitude = 30.0250m,
                Longitude = 31.4380m,
                IsDefault = false,
                IsActive = true
            });
        }

        // El Ezaby Locations
        var elezaby = clients.FirstOrDefault(c => c.Name.Contains("El Ezaby"));
        if (elezaby != null)
        {
            locations.Add(new ClientLocation
            {
                ClientId = elezaby.Id,
                Name = "El Ezaby - Abbas El Akkad Mega Branch (فرع عباس العقاد مدينة نصر)",
                Address = "105 Abbas El Akkad St, Zone 1, Nasr City",
                City = "Cairo",
                ContactName = "Dr. Ahmed Sallam (Branch Manager)",
                ContactPhone = "+20 101 999 8881",
                Latitude = 30.0590m,
                Longitude = 31.3410m,
                IsDefault = true,
                IsActive = true
            });
            locations.Add(new ClientLocation
            {
                ClientId = elezaby.Id,
                Name = "El Ezaby - Dokki Mossadak Branch (فرع مصدق الدقي)",
                Address = "48 Mossadak Street, Dokki",
                City = "Giza",
                ContactName = "Dr. Mina George (Operations Lead)",
                ContactPhone = "+20 101 999 7777",
                Latitude = 30.0390m,
                Longitude = 31.2080m,
                IsDefault = false,
                IsActive = true
            });
        }

        // B.TECH Locations
        var btech = clients.FirstOrDefault(c => c.Name.Contains("B.TECH"));
        if (btech != null)
        {
            locations.Add(new ClientLocation
            {
                ClientId = btech.Id,
                Name = "B.TECH - 90th Street Mega Store (فرع شارع التسعين التجمع)",
                Address = "Plot 120, Sector 1, South 90th St, New Cairo",
                City = "Cairo",
                ContactName = "Kareem Fouad (Showroom Operations)",
                ContactPhone = "+20 115 888 7772",
                Latitude = 30.0320m,
                Longitude = 31.4720m,
                IsDefault = true,
                IsActive = true
            });
            locations.Add(new ClientLocation
            {
                ClientId = btech.Id,
                Name = "B.TECH - Ahmed Orabi Mohandessin (فرع أحمد عرابي المهندسين)",
                Address = "12 Ahmed Orabi St, Mohandessin, Agouza",
                City = "Giza",
                ContactName = "Hany Shaker (Tech Store Supervisor)",
                ContactPhone = "+20 115 888 5555",
                Latitude = 30.0610m,
                Longitude = 31.2000m,
                IsDefault = false,
                IsActive = true
            });
        }

        // Metro Market Locations
        var metro = clients.FirstOrDefault(c => c.Name.Contains("Metro"));
        if (metro != null)
        {
            locations.Add(new ClientLocation
            {
                ClientId = metro.Id,
                Name = "Metro Market - Roxy Korba Branch (فرع روكسي مصر الجديدة)",
                Address = "9 Damascus Street, Roxy, Heliopolis",
                City = "Cairo",
                ContactName = "Sayed Abdelhamid (Store Coordinator)",
                ContactPhone = "+20 120 777 6663",
                Latitude = 30.0900m,
                Longitude = 31.3250m,
                IsDefault = true,
                IsActive = true
            });
            locations.Add(new ClientLocation
            {
                ClientId = metro.Id,
                Name = "Metro Market - Brazil Street Zamalek (فرع شارع البرازيل الزمالك)",
                Address = "15 Brazil St, Zamalek",
                City = "Cairo",
                ContactName = "Nader Samy (Branch Supervisor)",
                ContactPhone = "+20 120 777 4444",
                Latitude = 30.0630m,
                Longitude = 31.2180m,
                IsDefault = false,
                IsActive = true
            });
        }

        // Seif Pharmacies Locations
        var seif = clients.FirstOrDefault(c => c.Name.Contains("Seif"));
        if (seif != null)
        {
            locations.Add(new ClientLocation
            {
                ClientId = seif.Id,
                Name = "Seif Pharmacies - Maadi Degla Branch (فرع دجلة المعادي)",
                Address = "Road 206, Degla, Maadi",
                City = "Cairo",
                ContactName = "Dr. Mahmoud Helmy (Branch Manager)",
                ContactPhone = "+20 106 444 3335",
                Latitude = 29.9600m,
                Longitude = 31.2800m,
                IsDefault = true,
                IsActive = true
            });
        }

        if (locations.Any())
        {
            await context.ClientLocations.AddRangeAsync(locations);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedOrdersAsync(CleanSampleDbContext context)
    {
        if (await context.Orders.AnyAsync()) return;

        var clients = await context.Clients.ToListAsync();
        var carrefour = clients.FirstOrDefault(c => c.Name.Contains("Carrefour")) ?? clients.FirstOrDefault();
        var hyperone = clients.FirstOrDefault(c => c.Name.Contains("Hyperone")) ?? clients.FirstOrDefault();
        var elezaby = clients.FirstOrDefault(c => c.Name.Contains("El Ezaby")) ?? clients.FirstOrDefault();
        var btech = clients.FirstOrDefault(c => c.Name.Contains("B.TECH")) ?? clients.FirstOrDefault();

        if (carrefour == null) return;

        var orders = new List<Order>
        {
            new Order
            {
                ClientId = carrefour.Id,
                OrderDate = DateTime.UtcNow.AddDays(-6),
                RequiredDate = DateTime.UtcNow.AddDays(4),
                Status = (int)OrderStatusEnum.Processing,
                Notes = "Carrefour Ramadan FMCG promotional campaign: 15x FSDU units & 4x Lightboxes for Almaza & Maadi branches."
            },
            new Order
            {
                ClientId = hyperone?.Id ?? carrefour.Id,
                OrderDate = DateTime.UtcNow.AddDays(-4),
                RequiredDate = DateTime.UtcNow.AddDays(8),
                Status = (int)OrderStatusEnum.Processing,
                Notes = "Hyperone Sheikh Zayed renovation: Gondola end-cap cladding and 8x Hexagonal dump bins."
            },
            new Order
            {
                ClientId = elezaby?.Id ?? carrefour.Id,
                OrderDate = DateTime.UtcNow.AddDays(-2),
                RequiredDate = DateTime.UtcNow.AddDays(5),
                Status = (int)OrderStatusEnum.Pending,
                Notes = "El Ezaby Pharmacy skincare campaign: 20x Acrylic CDU 3-shelf counter displays & cosmetic glorifiers."
            },
            new Order
            {
                ClientId = btech?.Id ?? carrefour.Id,
                OrderDate = DateTime.UtcNow.AddDays(-1),
                RequiredDate = DateTime.UtcNow.AddDays(12),
                Status = (int)OrderStatusEnum.Processing,
                Notes = "B.TECH New Cairo showroom: 2x 43\" Interactive Touchscreen Kiosks & 6x SEG Lightboxes."
            }
        };

        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync();
    }

    private static async Task SeedOrderLinesAsync(CleanSampleDbContext context)
    {
        if (await context.OrderLines.AnyAsync()) return;

        var orders = await context.Orders.ToListAsync();
        var products = await context.Products.ToListAsync();

        if (!orders.Any() || !products.Any()) return;

        var orderLines = new List<OrderLine>();

        var fsdu = products.FirstOrDefault(p => p.Barcode == "622100010001") ?? products[0];
        var cdu = products.FirstOrDefault(p => p.Barcode == "622100010002") ?? products[0];
        var gondola = products.FirstOrDefault(p => p.Barcode == "622100010003") ?? products[0];
        var lightbox = products.FirstOrDefault(p => p.Barcode == "622100010004") ?? products[0];
        var kiosk = products.FirstOrDefault(p => p.Barcode == "622100010005") ?? products[0];
        var dumpbin = products.FirstOrDefault(p => p.Barcode == "622100010006") ?? products[0];

        // Order 1 (Carrefour)
        if (orders.Count > 0)
        {
            orderLines.Add(new OrderLine { OrderId = orders[0].Id, ProductId = fsdu.Id, Quantity = 15, Notes = "FSDU 4-Tier Displays for Almaza Store" });
            orderLines.Add(new OrderLine { OrderId = orders[0].Id, ProductId = lightbox.Id, Quantity = 4, Notes = "SEG 100x200cm Lightboxes with Ramadan graphics" });
        }

        // Order 2 (Hyperone)
        if (orders.Count > 1)
        {
            orderLines.Add(new OrderLine { OrderId = orders[1].Id, ProductId = gondola.Id, Quantity = 6, Notes = "Gondola End-Cap Claddings" });
            orderLines.Add(new OrderLine { OrderId = orders[1].Id, ProductId = dumpbin.Id, Quantity = 8, Notes = "Hexagonal Promotional Dump Bins" });
        }

        // Order 3 (El Ezaby)
        if (orders.Count > 2)
        {
            orderLines.Add(new OrderLine { OrderId = orders[2].Id, ProductId = cdu.Id, Quantity = 20, Notes = "Acrylic CDU 3-tier Counter Units" });
        }

        // Order 4 (B.TECH)
        if (orders.Count > 3)
        {
            orderLines.Add(new OrderLine { OrderId = orders[3].Id, ProductId = kiosk.Id, Quantity = 2, Notes = "43-inch Interactive Digital Totems" });
            orderLines.Add(new OrderLine { OrderId = orders[3].Id, ProductId = lightbox.Id, Quantity = 6, Notes = "SEG Fabric Lightboxes for Showroom Aisle" });
        }

        if (orderLines.Any())
        {
            await context.OrderLines.AddRangeAsync(orderLines);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedLoadRequestsAsync(CleanSampleDbContext context)
    {
        if (await context.LoadRequests.AnyAsync()) return;

        var orders = await context.Orders.ToListAsync();
        var clients = await context.Clients.ToListAsync();
        var locations = await context.ClientLocations.ToListAsync();
        var drivers = await context.Users.Where(u => u.UserName.Contains("driver")).ToListAsync();
        var supervisor = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("supervisor")) ?? await context.Users.FirstOrDefaultAsync();
        var vehicles = await context.Vehicles.ToListAsync();

        var carrefour = clients.FirstOrDefault(c => c.Name.Contains("Carrefour")) ?? clients.FirstOrDefault();
        var hyperone = clients.FirstOrDefault(c => c.Name.Contains("Hyperone")) ?? clients.FirstOrDefault();
        var btech = clients.FirstOrDefault(c => c.Name.Contains("B.TECH")) ?? clients.FirstOrDefault();

        var almazaLoc = locations.FirstOrDefault(l => l.Name.Contains("Almaza")) ?? locations.FirstOrDefault();
        var zayedLoc = locations.FirstOrDefault(l => l.Name.Contains("Zayed")) ?? locations.FirstOrDefault();
        var newCairoLoc = locations.FirstOrDefault(l => l.Name.Contains("90th Street")) ?? locations.FirstOrDefault();

        var truckJumbo = vehicles.FirstOrDefault(v => v.VehicleNumber == "VEH-EGY-01") ?? vehicles.FirstOrDefault();
        var pickupDmax = vehicles.FirstOrDefault(v => v.VehicleNumber == "VEH-EGY-02") ?? vehicles.FirstOrDefault();
        var vanHiAce = vehicles.FirstOrDefault(v => v.VehicleNumber == "VEH-EGY-04") ?? vehicles.FirstOrDefault();

        var driverAhmed = drivers.FirstOrDefault(d => d.UserName == "driver_ahmed") ?? drivers.FirstOrDefault();
        var driverMahmoud = drivers.FirstOrDefault(d => d.UserName == "driver_mahmoud") ?? drivers.FirstOrDefault();

        if (carrefour == null) return;

        var loadRequests = new List<LoadRequest>
        {
            new LoadRequest
            {
                OrderId = orders.ElementAtOrDefault(0)?.Id,
                ClientId = carrefour.Id,
                ClientLocationId = almazaLoc?.Id,
                RequestedBy = supervisor?.Id,
                RequestDate = DateTime.UtcNow.AddDays(-3),
                ExecutionDate = DateTime.UtcNow.AddDays(1),
                Status = (int)LoadRequestStatusEnum.New,
                DestinationAddress = "City Centre Almaza, Suez Road, Sheraton, Heliopolis",
                DestinationCity = "Cairo",
                Description = "Batch #1: 10x FSDU Displays & 2x SEG Lightboxes delivery to Carrefour Almaza",
                DriverId = driverAhmed?.Id,
                VehicleId = truckJumbo?.Id,
                Verified = false
            },
            new LoadRequest
            {
                OrderId = orders.ElementAtOrDefault(1)?.Id,
                ClientId = hyperone?.Id ?? carrefour.Id,
                ClientLocationId = zayedLoc?.Id,
                RequestedBy = supervisor?.Id,
                RequestDate = DateTime.UtcNow.AddDays(-2),
                ExecutionDate = DateTime.UtcNow.AddDays(2),
                Status = (int)LoadRequestStatusEnum.Loaded,
                DestinationAddress = "Entrance 1, 26th of July Corridor, Sheikh Zayed City",
                DestinationCity = "Giza",
                Description = "Gondola end-cap framing and 8x Hexagonal dump bins delivery for Hyperone Zayed",
                DriverId = driverMahmoud?.Id,
                VehicleId = pickupDmax?.Id,
                Verified = true
            },
            new LoadRequest
            {
                OrderId = orders.ElementAtOrDefault(3)?.Id,
                ClientId = btech?.Id ?? carrefour.Id,
                ClientLocationId = newCairoLoc?.Id,
                RequestedBy = supervisor?.Id,
                RequestDate = DateTime.UtcNow.AddDays(-1),
                ExecutionDate = DateTime.UtcNow.AddDays(3),
                Status = (int)LoadRequestStatusEnum.New,
                DestinationAddress = "Plot 120, South 90th St, New Cairo",
                DestinationCity = "Cairo",
                Description = "Delivery of 2x 43\" Interactive Kiosks and SEG lightboxes to B.TECH 90th Street",
                DriverId = driverAhmed?.Id,
                VehicleId = vanHiAce?.Id,
                Verified = false
            }
        };

        await context.LoadRequests.AddRangeAsync(loadRequests);
        await context.SaveChangesAsync();
    }

    private static async Task SeedLoadRequestLinesAsync(CleanSampleDbContext context)
    {
        if (await context.LoadRequestLines.AnyAsync()) return;

        var loadRequests = await context.LoadRequests.ToListAsync();
        var products = await context.Products.ToListAsync();

        if (!loadRequests.Any() || !products.Any()) return;

        var lines = new List<LoadRequestLine>();

        var fsdu = products.FirstOrDefault(p => p.Barcode == "622100010001") ?? products[0];
        var lightbox = products.FirstOrDefault(p => p.Barcode == "622100010004") ?? products[0];
        var gondola = products.FirstOrDefault(p => p.Barcode == "622100010003") ?? products[0];
        var dumpbin = products.FirstOrDefault(p => p.Barcode == "622100010006") ?? products[0];
        var kiosk = products.FirstOrDefault(p => p.Barcode == "622100010005") ?? products[0];

        // LR 1
        if (loadRequests.Count > 0)
        {
            lines.Add(new LoadRequestLine { LoadRequestId = loadRequests[0].Id, ProductId = fsdu.Id, Quantity = 10, CreatedAt = DateTime.UtcNow.AddDays(-3) });
            lines.Add(new LoadRequestLine { LoadRequestId = loadRequests[0].Id, ProductId = lightbox.Id, Quantity = 2, CreatedAt = DateTime.UtcNow.AddDays(-3) });
        }

        // LR 2
        if (loadRequests.Count > 1)
        {
            lines.Add(new LoadRequestLine { LoadRequestId = loadRequests[1].Id, ProductId = gondola.Id, Quantity = 4, CreatedAt = DateTime.UtcNow.AddDays(-2) });
            lines.Add(new LoadRequestLine { LoadRequestId = loadRequests[1].Id, ProductId = dumpbin.Id, Quantity = 8, CreatedAt = DateTime.UtcNow.AddDays(-2) });
        }

        // LR 3
        if (loadRequests.Count > 2)
        {
            lines.Add(new LoadRequestLine { LoadRequestId = loadRequests[2].Id, ProductId = kiosk.Id, Quantity = 2, CreatedAt = DateTime.UtcNow.AddDays(-1) });
            lines.Add(new LoadRequestLine { LoadRequestId = loadRequests[2].Id, ProductId = lightbox.Id, Quantity = 4, CreatedAt = DateTime.UtcNow.AddDays(-1) });
        }

        if (lines.Any())
        {
            await context.LoadRequestLines.AddRangeAsync(lines);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedLoadRequestPartsAsync(CleanSampleDbContext context)
    {
        if (await context.LoadRequestParts.AnyAsync()) return;

        var lines = await context.LoadRequestLines.ToListAsync();
        var boms = await context.ProductBOMs.ToListAsync();

        if (!lines.Any()) return;

        var lrParts = new List<LoadRequestPart>();

        foreach (var line in lines)
        {
            var matchingBoms = boms.Where(b => b.ProductId == line.ProductId).ToList();
            foreach (var bom in matchingBoms)
            {
                var reqQty = line.Quantity * bom.Quantity;
                lrParts.Add(new LoadRequestPart
                {
                    LoadRequestId = line.LoadRequestId,
                    LoadRequestLineId = line.Id,
                    ProductId = line.ProductId,
                    PartId = bom.PartId,
                    RequiredQuantity = reqQty,
                    LoadedQuantity = reqQty,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        if (lrParts.Any())
        {
            await context.LoadRequestParts.AddRangeAsync(lrParts);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedVehicleLoadsAsync(CleanSampleDbContext context)
    {
        if (await context.VehicleLoads.AnyAsync()) return;

        var lrParts = await context.LoadRequestParts.Take(8).ToListAsync();
        var techUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "tech_mostafa") ?? await context.Users.FirstOrDefaultAsync();
        var driverUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "driver_ahmed") ?? await context.Users.FirstOrDefaultAsync();
        var vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.VehicleNumber == "VEH-EGY-01") ?? await context.Vehicles.FirstOrDefaultAsync();

        if (!lrParts.Any()) return;

        var loads = new List<VehicleLoad>();
        int index = 1;

        foreach (var lrPart in lrParts)
        {
            loads.Add(new VehicleLoad
            {
                LoadRequestId = lrPart.LoadRequestId,
                PartId = lrPart.PartId,
                Barcode = $"EGY-LOAD-{DateTime.UtcNow:yyyyMMdd}-{index:D4}",
                Quantity = lrPart.RequiredQuantity,
                LoadedBy = techUser?.Id,
                DriverId = driverUser?.Id,
                VehicleId = vehicle?.Id,
                LoadDate = DateTime.UtcNow.AddHours(-12),
                Status = (int)VehicleLoadStatusEnum.Good,
                Notes = $"Verified pallet scan #{index} for Cairo dispatch"
            });
            index++;
        }

        await context.VehicleLoads.AddRangeAsync(loads);
        await context.SaveChangesAsync();
    }

    private static async Task SeedVehicleOffloadsAsync(CleanSampleDbContext context)
    {
        if (await context.VehicleOffloads.AnyAsync()) return;

        var loadRequest = await context.LoadRequests.FirstOrDefaultAsync();
        var vehicle = await context.Vehicles.FirstOrDefaultAsync();
        var driver = await context.Users.FirstOrDefaultAsync(u => u.UserName == "driver_ahmed") ?? await context.Users.FirstOrDefaultAsync();
        var supervisor = await context.Users.FirstOrDefaultAsync(u => u.UserName == "supervisor_tarek") ?? await context.Users.FirstOrDefaultAsync();
        var part = await context.Parts.FirstOrDefaultAsync();

        if (loadRequest == null || vehicle == null || driver == null || part == null) return;

        var offloads = new List<VehicleOffload>
        {
            new VehicleOffload
            {
                LoadRequestId = loadRequest.Id,
                PartId = part.Id,
                VehicleId = vehicle.Id,
                DriverId = driver.Id,
                Barcode = "EGY-OFFLOAD-0001",
                OffloadDate = DateTime.UtcNow.AddHours(-2),
                Status = (int)VehicleOffloadStatusEnum.Good,
                Verified = true,
                VerifiedBy = supervisor?.Id,
                VerifiedAt = DateTime.UtcNow.AddHours(-1),
                Notes = "Delivered and verified at Carrefour Almaza receiving dock #3"
            }
        };

        await context.VehicleOffloads.AddRangeAsync(offloads);
        await context.SaveChangesAsync();
    }

    private static async Task SeedFieldJobsAsync(CleanSampleDbContext context)
    {
        if (await context.FieldJobs.AnyAsync()) return;

        var loadRequest = await context.LoadRequests.FirstOrDefaultAsync();
        var client = await context.Clients.FirstOrDefaultAsync(c => c.Name.Contains("Carrefour")) ?? await context.Clients.FirstOrDefaultAsync();
        var clientLocation = await context.ClientLocations.FirstOrDefaultAsync(l => l.Name.Contains("Almaza")) ?? await context.ClientLocations.FirstOrDefaultAsync();
        var technician = await context.Users.FirstOrDefaultAsync(u => u.UserName == "tech_mostafa") ?? await context.Users.FirstOrDefaultAsync();
        var supervisor = await context.Users.FirstOrDefaultAsync(u => u.UserName == "supervisor_tarek") ?? await context.Users.FirstOrDefaultAsync();

        if (loadRequest == null || client == null) return;

        var jobs = new List<FieldJob>
        {
            new FieldJob
            {
                LoadRequestId = loadRequest.Id,
                ClientId = client.Id,
                ClientLocationId = clientLocation?.Id,
                TechnicianId = technician?.Id,
                SupervisorId = supervisor?.Id,
                ScheduledDate = DateTime.UtcNow.AddDays(1),
                StartDate = DateTime.UtcNow.AddDays(1).AddHours(2),
                CompletionDate = null,
                Status = (int)FieldJobStatusEnum.InProgress,
                Verified = false,
                Notes = "On-site assembly and merchandising setup for 10x FSDU displays at Carrefour Almaza FMCG main aisle"
            },
            new FieldJob
            {
                LoadRequestId = loadRequest.Id,
                ClientId = client.Id,
                ClientLocationId = clientLocation?.Id,
                TechnicianId = technician?.Id,
                SupervisorId = supervisor?.Id,
                ScheduledDate = DateTime.UtcNow.AddDays(3),
                StartDate = null,
                CompletionDate = null,
                Status = (int)FieldJobStatusEnum.Scheduled,
                Verified = false,
                Notes = "Installation of 2x SEG Frameless Fabric Lightboxes on Carrefour entrance pillars"
            }
        };

        await context.FieldJobs.AddRangeAsync(jobs);
        await context.SaveChangesAsync();
    }

    private static async Task SeedFieldAssembliesAsync(CleanSampleDbContext context)
    {
        if (await context.FieldAssemblies.AnyAsync()) return;

        var job = await context.FieldJobs.FirstOrDefaultAsync();
        var product = await context.Products.FirstOrDefaultAsync(p => p.Barcode == "622100010001") ?? await context.Products.FirstOrDefaultAsync();
        var technician = await context.Users.FirstOrDefaultAsync(u => u.UserName == "tech_mostafa") ?? await context.Users.FirstOrDefaultAsync();
        var supervisor = await context.Users.FirstOrDefaultAsync(u => u.UserName == "supervisor_tarek") ?? await context.Users.FirstOrDefaultAsync();

        if (job == null || product == null) return;

        var assemblies = new List<FieldAssembly>
        {
            new FieldAssembly
            {
                FieldJobId = job.Id,
                ProductId = product.Id,
                ProductBarcode = "EGY-FSDU-2026-001",
                Quantity = 1,
                AssemblyDate = DateTime.UtcNow,
                Status = (int)FieldAssemblyStatusEnum.Completed,
                TechnicianId = technician?.Id,
                SupervisorId = supervisor?.Id,
                Verified = true,
                VerifiedAt = DateTime.UtcNow,
                Notes = "FSDU floor display assembled and stocked with promotional merchandise at Carrefour Almaza aisle 4"
            },
            new FieldAssembly
            {
                FieldJobId = job.Id,
                ProductId = product.Id,
                ProductBarcode = "EGY-FSDU-2026-002",
                Quantity = 1,
                AssemblyDate = null,
                Status = (int)FieldAssemblyStatusEnum.InProgress,
                TechnicianId = technician?.Id,
                SupervisorId = supervisor?.Id,
                Verified = false,
                Notes = "Assembling steel frame and attaching Forex side panels"
            }
        };

        await context.FieldAssemblies.AddRangeAsync(assemblies);
        await context.SaveChangesAsync();
    }

    private static async Task SeedIssuesAsync(CleanSampleDbContext context)
    {
        if (await context.Issues.AnyAsync()) return;

        var loadRequest = await context.LoadRequests.FirstOrDefaultAsync();
        var fieldJob = await context.FieldJobs.FirstOrDefaultAsync();
        var fieldAssembly = await context.FieldAssemblies.FirstOrDefaultAsync();
        var reporter = await context.Users.FirstOrDefaultAsync(u => u.UserName == "tech_mostafa") ?? await context.Users.FirstOrDefaultAsync();
        var resolver = await context.Users.FirstOrDefaultAsync(u => u.UserName == "supervisor_tarek") ?? await context.Users.FirstOrDefaultAsync();

        var issues = new List<Issue>
        {
            new Issue
            {
                LoadRequestId = loadRequest?.Id,
                FieldJobId = fieldJob?.Id,
                FieldAssemblyId = fieldAssembly?.Id,
                IssueType = "Site Clearance & Outlet Distance",
                Description = "Power outlet at Carrefour Almaza pillar was 4 meters away from lightbox installation point. Required 5m extension cable.",
                Severity = "Medium",
                Status = (int)IssueStatusEnum.Resolved,
                ReportedBy = reporter?.Id,
                ReportedAt = DateTime.UtcNow.AddDays(-1),
                ResolvedBy = resolver?.Id,
                ResolvedAt = DateTime.UtcNow,
                ResolutionNotes = "Installed heavy-duty insulated extension line with cable trunking along baseboard. Passed store safety inspection."
            },
            new Issue
            {
                LoadRequestId = loadRequest?.Id,
                FieldJobId = fieldJob?.Id,
                FieldAssemblyId = null,
                IssueType = "Traffic & Delivery Gate Delay",
                Description = "Heavy congestion on Ring Road near Almaza interchange caused a 40-minute delay at receiving gate #3.",
                Severity = "Low",
                Status = (int)IssueStatusEnum.Closed,
                ReportedBy = reporter?.Id,
                ReportedAt = DateTime.UtcNow,
                ResolvedBy = resolver?.Id,
                ResolvedAt = DateTime.UtcNow,
                ResolutionNotes = "Driver coordinated with store dock receiving supervisor; unloading completed smoothly."
            }
        };

        await context.Issues.AddRangeAsync(issues);
        await context.SaveChangesAsync();
    }

    private static async Task SeedScreensAsync(CleanSampleDbContext context)
    {
        var screensCount = await context.Screens.CountAsync();
        if (screensCount == 0)
        {
            var screens = new List<Screen>
            {
                new Screen { Code = "USERS", NameEn = "Users Management", NameAr = "إدارة المستخدمين", Module = "Administration", DescriptionEn = "Manage system users and active states", DescriptionAr = "إدارة مستخدمي النظام وحالات التفعيل" },
                new Screen { Code = "ROLES", NameEn = "Roles", NameAr = "الأدوار والصلاحيات", Module = "Administration", DescriptionEn = "Configure roles and access rights", DescriptionAr = "تهيئة الأدوار وصلاحيات الوصول" },
                new Screen { Code = "ROLE_PERMISSIONS", NameEn = "Role Permissions", NameAr = "صلاحيات الأدوار والشاشات", Module = "Administration", DescriptionEn = "Configure role access rights and screen permissions", DescriptionAr = "تهيئة صلاحيات الأدوار وشاشات الوصول" },
                new Screen { Code = "SCREENS", NameEn = "System Screens", NameAr = "شاشات النظام", Module = "Administration", DescriptionEn = "Manage system screen registry", DescriptionAr = "إدارة سجل شاشات النظام" },

                new Screen { Code = "CATEGORIES", NameEn = "Categories", NameAr = "التصنيفات", Module = "Master Data", DescriptionEn = "Manage item categories", DescriptionAr = "إدارة تصنيفات الأصناف" },
                new Screen { Code = "COLORS", NameEn = "Colors", NameAr = "الألوان", Module = "Master Data", DescriptionEn = "Manage item colors", DescriptionAr = "إدارة ألوان الأصناف" },
                new Screen { Code = "MATERIALS", NameEn = "Materials", NameAr = "المواد والخامات", Module = "Master Data", DescriptionEn = "Manage item materials", DescriptionAr = "إدارة خامات ومواد الأصناف" },
                new Screen { Code = "DESIGNS", NameEn = "Designs", NameAr = "التصميمات والنماذج", Module = "Master Data", DescriptionEn = "Manage item designs", DescriptionAr = "إدارة تصميمات ونماذج الأصناف" },

                new Screen { Code = "PRODUCTS", NameEn = "POSM Products Catalog", NameAr = "كتالوج منتجات الدعاية (POSM)", Module = "Inventory", DescriptionEn = "Manage POSM master products catalog", DescriptionAr = "إدارة كتالوج منتجات ومواد الدعاية والإعلان الرئيسية" },
                new Screen { Code = "PARTS", NameEn = "Parts & Components", NameAr = "القطع والمكونات", Module = "Inventory", DescriptionEn = "Manage POSM parts and raw components inventory", DescriptionAr = "إدارة مخزون قطع الغيار والمكونات الخام لمنتجات POSM" },
                new Screen { Code = "PRODUCT_BOM", NameEn = "Product BOM", NameAr = "قائمة مواد وتركيب المنتج (BOM)", Module = "Inventory", DescriptionEn = "Manage bill of materials composition", DescriptionAr = "إدارة قائمة تركيب ومواد المنتج" },

                new Screen { Code = "CLIENTS", NameEn = "Clients & Retailers", NameAr = "العملاء وسلاسل التجزئة", Module = "Sales", DescriptionEn = "Manage client accounts and retail chains", DescriptionAr = "إدارة حسابات العملاء وسلاسل المتاجر" },
                new Screen { Code = "CLIENT_LOCATIONS", NameEn = "Client Store Locations", NameAr = "فروع ومواقع المتاجر للعملاء", Module = "Sales", DescriptionEn = "Manage client store branches and delivery sites", DescriptionAr = "إدارة عناوين وفروع تسليم المتاجر للعملاء" },
                new Screen { Code = "ORDERS", NameEn = "Sales Orders", NameAr = "أوامر المبيعات والتوريد", Module = "Sales", DescriptionEn = "Manage sales orders and project lines", DescriptionAr = "إدارة أوامر المبيعات والتوريد وبنود المشاريع" },

                new Screen { Code = "LOAD_REQUESTS", NameEn = "Load Requests", NameAr = "طلبات التحميل للمستودع", Module = "Warehouse", DescriptionEn = "Manage warehouse load allocations", DescriptionAr = "إدارة طلبات ومخصصات التحميل بالمستودع" },
                new Screen { Code = "VEHICLE_LOADS", NameEn = "Vehicle Loads", NameAr = "تحميل المركبات والسيارات", Module = "Warehouse", DescriptionEn = "Vehicle load scanning and logging", DescriptionAr = "مسح وتسجيل عمليات تحميل المركبات" },

                new Screen { Code = "VEHICLES", NameEn = "Fleet Vehicles", NameAr = "أسطول المركبات", Module = "Logistics", DescriptionEn = "Fleet transport vehicle tracking", DescriptionAr = "إدارة وتتبع سيارات ومركبات أسطول النقل" },
                new Screen { Code = "VEHICLE_OFFLOADS", NameEn = "Vehicle Offloads", NameAr = "تنزيل حمولات المركبات", Module = "Logistics", DescriptionEn = "Manage store offloading manifests", DescriptionAr = "إدارة وتوثيق تنزيل حمولات المركبات بالفروع" },

                new Screen { Code = "FIELD_JOBS", NameEn = "Field Service Jobs", NameAr = "مهام التركيبات الميدانية", Module = "Operations", DescriptionEn = "Manage field installation work orders", DescriptionAr = "إدارة أوامر مهام التركيب والخدمات الميدانية" },
                new Screen { Code = "FIELD_ASSEMBLIES", NameEn = "Field Assemblies", NameAr = "تجميع وتركيب المنتجات ميدانياً", Module = "Operations", DescriptionEn = "Manage on-site product assemblies", DescriptionAr = "إدارة وتوثيق تجميع وتركيب المنتجات ميدانياً بالفروع" },
                new Screen { Code = "ISSUES", NameEn = "Issue Tracking & Incidents", NameAr = "متابعة البلاغات والمشكلات الفنية", Module = "Operations", DescriptionEn = "Incident reporting and defect logging", DescriptionAr = "تسجيل ومتابعة البلاغات والعيوب الفنية" }
            };

            await context.Screens.AddRangeAsync(screens);
            await context.SaveChangesAsync();
        }
        else
        {
            if (!await context.Screens.AnyAsync(s => s.Code == "ROLE_PERMISSIONS"))
            {
                context.Screens.Add(new Screen
                {
                    Code = "ROLE_PERMISSIONS",
                    NameEn = "Role Permissions",
                    NameAr = "صلاحيات الأدوار والشاشات",
                    Module = "Administration",
                    DescriptionEn = "Configure role access rights and screen permissions",
                    DescriptionAr = "تهيئة صلاحيات الأدوار وشاشات الوصول",
                    IsActive = true
                });
                await context.SaveChangesAsync();
            }
        }
    }

    private static async Task SeedRolePermissionsAsync(CleanSampleDbContext context)
    {
        var roles = await context.Roles.ToListAsync();
        var screens = await context.Screens.ToListAsync();

        var adminRole = roles.FirstOrDefault(r => r.NameEn == "Admin" || r.NameAr == "مدير النظام");
        var managerRole = roles.FirstOrDefault(r => r.NameEn == "Manager" || r.NameAr == "مدير");
        var supervisorRole = roles.FirstOrDefault(r => r.NameEn == "Supervisor" || r.NameAr == "مشرف");
        var technicianRole = roles.FirstOrDefault(r => r.NameEn == "Technician" || r.NameAr == "فني");
        var driverRole = roles.FirstOrDefault(r => r.NameEn == "Driver" || r.NameAr == "سائق");
        var userRole = roles.FirstOrDefault(r => r.NameEn == "User" || r.NameAr == "مستخدم");

        var permissionsCount = await context.RolePermissions.CountAsync();
        if (permissionsCount == 0)
        {
            var permissions = new List<RolePermission>();

            foreach (var screen in screens)
            {
                // Admin: Full access to everything
                if (adminRole != null)
                {
                    permissions.Add(new RolePermission
                    {
                        RoleId = adminRole.Id,
                        ScreenId = screen.Id,
                        CanView = true,
                        CanCreate = true,
                        CanUpdate = true,
                        CanDelete = true
                    });
                }

                // Manager: View everything, Create/Update on non-admin
                if (managerRole != null)
                {
                    var isAdministration = screen.Module == "Administration";
                    permissions.Add(new RolePermission
                    {
                        RoleId = managerRole.Id,
                        ScreenId = screen.Id,
                        CanView = true,
                        CanCreate = !isAdministration,
                        CanUpdate = !isAdministration,
                        CanDelete = screen.Module is "Sales" or "Inventory"
                    });
                }

                // Supervisor: Full on Operations & Warehouse & Logistics
                if (supervisorRole != null)
                {
                    var isOpsOrLogistics = screen.Module is "Operations" or "Logistics" or "Warehouse";
                    permissions.Add(new RolePermission
                    {
                        RoleId = supervisorRole.Id,
                        ScreenId = screen.Id,
                        CanView = isOpsOrLogistics || screen.Module == "Inventory",
                        CanCreate = isOpsOrLogistics,
                        CanUpdate = isOpsOrLogistics,
                        CanDelete = screen.Module == "Operations"
                    });
                }

                // Technician: Operations & Inventory
                if (technicianRole != null)
                {
                    var isTechScreen = screen.Code is "FIELD_JOBS" or "FIELD_ASSEMBLIES" or "ISSUES" or "PRODUCTS" or "PARTS";
                    var canUpdate = screen.Code is "FIELD_ASSEMBLIES" or "ISSUES";
                    permissions.Add(new RolePermission
                    {
                        RoleId = technicianRole.Id,
                        ScreenId = screen.Id,
                        CanView = isTechScreen,
                        CanCreate = screen.Code is "ISSUES" or "FIELD_ASSEMBLIES",
                        CanUpdate = canUpdate,
                        CanDelete = false
                    });
                }

                // Driver: Logistics & Issues
                if (driverRole != null)
                {
                    var isDriverScreen = screen.Code is "VEHICLES" or "VEHICLE_OFFLOADS" or "ISSUES";
                    permissions.Add(new RolePermission
                    {
                        RoleId = driverRole.Id,
                        ScreenId = screen.Id,
                        CanView = isDriverScreen,
                        CanCreate = screen.Code == "ISSUES",
                        CanUpdate = screen.Code is "VEHICLE_OFFLOADS" or "ISSUES",
                        CanDelete = false
                    });
                }

                // User: View only on Products, Orders, Categories
                if (userRole != null)
                {
                    var isUserScreen = screen.Code is "PRODUCTS" or "CATEGORIES" or "ORDERS";
                    permissions.Add(new RolePermission
                    {
                        RoleId = userRole.Id,
                        ScreenId = screen.Id,
                        CanView = isUserScreen,
                        CanCreate = false,
                        CanUpdate = false,
                        CanDelete = false
                    });
                }
            }

            await context.RolePermissions.AddRangeAsync(permissions);
            await context.SaveChangesAsync();
        }
        else
        {
            var rolePermissionsScreen = screens.FirstOrDefault(s => s.Code == "ROLE_PERMISSIONS");
            if (rolePermissionsScreen != null && adminRole != null)
            {
                var existingAdminPermission = await context.RolePermissions
                    .FirstOrDefaultAsync(rp => rp.RoleId == adminRole.Id && rp.ScreenId == rolePermissionsScreen.Id);

                if (existingAdminPermission == null)
                {
                    context.RolePermissions.Add(new RolePermission
                    {
                        RoleId = adminRole.Id,
                        ScreenId = rolePermissionsScreen.Id,
                        CanView = true,
                        CanCreate = true,
                        CanUpdate = true,
                        CanDelete = true
                    });
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}