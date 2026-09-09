using CleanSample.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

/// <summary>
/// Database seeder for populating all tables with realistic dummy data
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
            // 1. Roles
            await SeedRolesAsync(context);

            // 2. Users
            await SeedUsersAsync(context);

            // 3. Lookup Tables (Categories, Colors, Materials, Designs)
            await SeedCategoriesAsync(context);
            await SeedColorsAsync(context);
            await SeedMaterialsAsync(context);
            await SeedDesignsAsync(context);

            // 4. Products & ProductVariants
            await SeedProductsAsync(context);
            await SeedProductVariantsAsync(context);

            // 5. Parts & ProductBOM
            await SeedPartsAsync(context);
            await SeedProductBOMAsync(context);

            // 6. Vehicles
            await SeedVehiclesAsync(context);

            // 7. Clients & ClientLocations
            await SeedClientsAsync(context);
            await SeedClientLocationsAsync(context);

            // 8. Orders & OrderLines
            await SeedOrdersAsync(context);
            await SeedOrderLinesAsync(context);

            // 9. PickRequests, PickRequestLines, PickRequestParts
            await SeedPickRequestsAsync(context);
            await SeedPickRequestLinesAsync(context);
            await SeedPickRequestPartsAsync(context);

            // 10. Picks
            await SeedPicksAsync(context);

            // 11. VehicleLoads & VehicleLoadItems
            await SeedVehicleLoadsAsync(context);
            await SeedVehicleLoadItemsAsync(context);

            // 12. FieldJobs & FieldAssemblies
            await SeedFieldJobsAsync(context);
            await SeedFieldAssembliesAsync(context);

            // 13. Issues
            await SeedIssuesAsync(context);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error occurred while seeding the database", ex);
        }
    }

    private static async Task SeedRolesAsync(CleanSampleDbContext context)
    {
        if (await context.Roles.AnyAsync()) return;

        var roles = new List<Role>
        {
            new Role { Name = "Admin" },
            new Role { Name = "User" },
            new Role { Name = "Manager" },
            new Role { Name = "Driver" },
            new Role { Name = "Technician" },
            new Role { Name = "Supervisor" }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(CleanSampleDbContext context)
    {
        var existingUsersCount = await context.Users.CountAsync();
        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        var managerRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Manager");
        var driverRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Driver");
        var technicianRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Technician");
        var supervisorRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Supervisor");

        if (existingUsersCount == 0)
        {
            var users = new List<User>
            {
                new User
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    FullName = "Administrator",
                    RoleId = adminRole?.Id,
                    Mobile = "555-0001",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    UserName = "user",
                    Email = "user@example.com",
                    FullName = "Regular User",
                    RoleId = userRole?.Id,
                    Mobile = "555-0002",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    UserName = "manager",
                    Email = "manager@example.com",
                    FullName = "Manager User",
                    RoleId = managerRole?.Id,
                    Mobile = "555-0003",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        // Ensure operational users (Drivers, Technicians, Supervisors) exist for foreign keys
        if (!await context.Users.AnyAsync(u => u.UserName == "driver_john"))
        {
            context.Users.Add(new User
            {
                UserName = "driver_john",
                Email = "john.driver@example.com",
                FullName = "John Driver",
                RoleId = driverRole?.Id,
                Mobile = "555-0101",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!await context.Users.AnyAsync(u => u.UserName == "driver_mike"))
        {
            context.Users.Add(new User
            {
                UserName = "driver_mike",
                Email = "mike.driver@example.com",
                FullName = "Mike Driver",
                RoleId = driverRole?.Id,
                Mobile = "555-0102",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!await context.Users.AnyAsync(u => u.UserName == "tech_alex"))
        {
            context.Users.Add(new User
            {
                UserName = "tech_alex",
                Email = "alex.tech@example.com",
                FullName = "Alex Technician",
                RoleId = technicianRole?.Id,
                Mobile = "555-0201",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!await context.Users.AnyAsync(u => u.UserName == "tech_sam"))
        {
            context.Users.Add(new User
            {
                UserName = "tech_sam",
                Email = "sam.tech@example.com",
                FullName = "Sam Technician",
                RoleId = technicianRole?.Id,
                Mobile = "555-0202",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!await context.Users.AnyAsync(u => u.UserName == "supervisor_robert"))
        {
            context.Users.Add(new User
            {
                UserName = "supervisor_robert",
                Email = "robert.supervisor@example.com",
                FullName = "Robert Supervisor",
                RoleId = supervisorRole?.Id,
                Mobile = "555-0301",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedCategoriesAsync(CleanSampleDbContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        var categories = new List<Category>
        {
            new Category { Name = "Electronics", Description = "Electronic devices and computing accessories" },
            new Category { Name = "Furniture", Description = "Office and warehouse ergonomic furniture" },
            new Category { Name = "Hardware", Description = "Mechanical and hardware structural parts" },
            new Category { Name = "Apparel", Description = "Industrial protective apparel and gear" },
            new Category { Name = "Tools", Description = "Assembly and maintenance hand tools" }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }

    private static async Task SeedColorsAsync(CleanSampleDbContext context)
    {
        if (await context.Colors.AnyAsync()) return;

        var colors = new List<Color>
        {
            new Color { Name = "Black", Code = "#000000" },
            new Color { Name = "Silver", Code = "#C0C0C0" },
            new Color { Name = "White", Code = "#FFFFFF" },
            new Color { Name = "Midnight Blue", Code = "#191970" },
            new Color { Name = "Space Gray", Code = "#808080" },
            new Color { Name = "Crimson Red", Code = "#DC143C" }
        };

        await context.Colors.AddRangeAsync(colors);
        await context.SaveChangesAsync();
    }

    private static async Task SeedMaterialsAsync(CleanSampleDbContext context)
    {
        if (await context.Materials.AnyAsync()) return;

        var materials = new List<Material>
        {
            new Material { Name = "Aluminum", Description = "Lightweight anodized aluminum alloy" },
            new Material { Name = "Carbon Fiber", Description = "High-strength composite carbon fiber" },
            new Material { Name = "Stainless Steel", Description = "Corrosion resistant grade 304 steel" },
            new Material { Name = "Polycarbonate", Description = "Impact resistant engineering plastic" },
            new Material { Name = "Silicone", Description = "Flexible heat-resistant polymer" }
        };

        await context.Materials.AddRangeAsync(materials);
        await context.SaveChangesAsync();
    }

    private static async Task SeedDesignsAsync(CleanSampleDbContext context)
    {
        if (await context.Designs.AnyAsync()) return;

        var designs = new List<Design>
        {
            new Design { Name = "Modern Minimalist", Code = "MOD-MIN", Description = "Clean lines and sleek form factor" },
            new Design { Name = "Industrial Rugged", Code = "IND-RUG", Description = "Reinforced corners and shock resistance" },
            new Design { Name = "Ergonomic Pro", Code = "ERG-PRO", Description = "Designed for high comfort and prolonged usage" },
            new Design { Name = "Compact Slim", Code = "CMP-SLM", Description = "Ultra-thin portable profile" }
        };

        await context.Designs.AddRangeAsync(designs);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductsAsync(CleanSampleDbContext context)
    {
        if (await context.Products.AnyAsync()) return;

        var category = await context.Categories.FirstOrDefaultAsync() ?? new Category { Name = "Electronics" };
        if (category.Id == 0)
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
        }

        var products = new List<Product>
        {
            new Product { Name = "Pro Laptop 15", Description = "Flagship 15-inch development workstation", Price = 1499.99m, Stock = 50, CategoryId = category.Id },
            new Product { Name = "Smart Phone X", Description = "5G enterprise smartphone", Price = 899.99m, Stock = 100, CategoryId = category.Id },
            new Product { Name = "Industrial Tablet", Description = "Rugged waterproof tablet for field technicians", Price = 649.99m, Stock = 75, CategoryId = category.Id },
            new Product { Name = "UltraWide Monitor", Description = "34-inch curved productivity monitor", Price = 599.99m, Stock = 40, CategoryId = category.Id },
            new Product { Name = "Mechanical Keyboard", Description = "RGB wireless mechanical keyboard", Price = 129.99m, Stock = 200, CategoryId = category.Id }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductVariantsAsync(CleanSampleDbContext context)
    {
        if (await context.ProductVariants.AnyAsync()) return;

        var products = await context.Products.Take(5).ToListAsync();
        if (!products.Any()) return;

        var colors = await context.Colors.Take(3).ToListAsync();
        var materials = await context.Materials.Take(3).ToListAsync();
        var designs = await context.Designs.Take(2).ToListAsync();

        var variants = new List<ProductVariant>();
        int index = 1;

        foreach (var prod in products)
        {
            for (int i = 0; i < Math.Min(2, colors.Count); i++)
            {
                var color = colors[i];
                var material = materials.ElementAtOrDefault(i) ?? materials.FirstOrDefault();
                var design = designs.ElementAtOrDefault(i % designs.Count) ?? designs.FirstOrDefault();

                variants.Add(new ProductVariant
                {
                    ProductId = prod.Id,
                    ColorId = color?.Id,
                    MaterialId = material?.Id,
                    DesignId = design?.Id,
                    Code = $"VAR-{prod.Id}-{index:D3}",
                    Barcode = $"890123456{index:D3}",
                    Description = $"{prod.Name} - {color?.Name ?? "Standard"} / {material?.Name ?? "Standard"}",
                    IsActive = true
                });
                index++;
            }
        }

        await context.ProductVariants.AddRangeAsync(variants);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPartsAsync(CleanSampleDbContext context)
    {
        if (await context.Parts.AnyAsync()) return;

        var parts = new List<Part>
        {
            new Part { Code = "PART-CPU-01", Name = "Core Processor Module", Description = "High performance octa-core CPU", Barcode = "BC-CPU-1001", IsActive = true },
            new Part { Code = "PART-RAM-16", Name = "16GB Memory Module", Description = "DDR5 4800MHz RAM stick", Barcode = "BC-RAM-1002", IsActive = true },
            new Part { Code = "PART-SSD-512", Name = "512GB Solid State Drive", Description = "M.2 NVMe PCIe Gen4 SSD", Barcode = "BC-SSD-1003", IsActive = true },
            new Part { Code = "PART-BAT-70W", Name = "70Wh Li-Ion Battery", Description = "High-density lithium polymer battery pack", Barcode = "BC-BAT-1004", IsActive = true },
            new Part { Code = "PART-SCR-15", Name = "15.6 Inch IPS Panel", Description = "Anti-glare 4K UHD display panel", Barcode = "BC-SCR-1005", IsActive = true },
            new Part { Code = "PART-CHAS-01", Name = "Unibody Metal Chassis", Description = "CNC precision machined aluminum frame", Barcode = "BC-CHS-1006", IsActive = true },
            new Part { Code = "PART-KBD-01", Name = "Backlit Keyboard Assembly", Description = "Scissor-switch LED illuminated keyboard", Barcode = "BC-KBD-1007", IsActive = true },
            new Part { Code = "PART-MTH-01", Name = "Main Motherboard PCB", Description = "Multi-layer PCB system board", Barcode = "BC-MTH-1008", IsActive = true }
        };

        await context.Parts.AddRangeAsync(parts);
        await context.SaveChangesAsync();
    }

    private static async Task SeedProductBOMAsync(CleanSampleDbContext context)
    {
        if (await context.ProductBOMs.AnyAsync()) return;

        var variants = await context.ProductVariants.Take(5).ToListAsync();
        var parts = await context.Parts.ToListAsync();

        if (!variants.Any() || !parts.Any()) return;

        var boms = new List<ProductBOM>();

        foreach (var variant in variants)
        {
            // Assign 3-5 parts per variant
            for (int i = 0; i < Math.Min(4, parts.Count); i++)
            {
                var part = parts[i];
                boms.Add(new ProductBOM
                {
                    ProductVariantId = variant.Id,
                    PartId = part.Id,
                    Quantity = (i % 2 == 0) ? 1.0m : 2.0m
                });
            }
        }

        await context.ProductBOMs.AddRangeAsync(boms);
        await context.SaveChangesAsync();
    }

    private static async Task SeedVehiclesAsync(CleanSampleDbContext context)
    {
        if (await context.Vehicles.AnyAsync()) return;

        var vehicles = new List<Vehicle>
        {
            new Vehicle { VehicleNumber = "VEH-101", PlateNumber = "ABC-1234", VehicleType = "Cargo Van", CapacityKg = 1500.00m, IsActive = true },
            new Vehicle { VehicleNumber = "VEH-102", PlateNumber = "XYZ-5678", VehicleType = "Heavy Duty Truck", CapacityKg = 5000.00m, IsActive = true },
            new Vehicle { VehicleNumber = "VEH-103", PlateNumber = "KTM-9988", VehicleType = "Delivery Pickup", CapacityKg = 2200.00m, IsActive = true },
            new Vehicle { VehicleNumber = "VEH-104", PlateNumber = "DXB-7744", VehicleType = "Electric Transit Van", CapacityKg = 1800.00m, IsActive = true }
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
                Code = "CL-TECH-01",
                Name = "Apex Technology Solutions",
                Phone = "1-800-555-0199",
                Mobile = "555-111-2222",
                Email = "procurement@apextech.io",
                Address = "100 Innovation Way, Suite 400",
                City = "New York",
                IsActive = true
            },
            new Client
            {
                Code = "CL-GLOB-02",
                Name = "Global Logistics Network",
                Phone = "1-800-555-0288",
                Mobile = "555-333-4444",
                Email = "orders@globallogistics.com",
                Address = "250 Freight Terminal Blvd",
                City = "Chicago",
                IsActive = true
            },
            new Client
            {
                Code = "CL-NEXUS-03",
                Name = "Nexus Industrial Corp",
                Phone = "1-800-555-0377",
                Mobile = "555-555-6666",
                Email = "supply@nexusind.com",
                Address = "88 Industrial Parkway",
                City = "Houston",
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

        foreach (var client in clients)
        {
            locations.Add(new ClientLocation
            {
                ClientId = client.Id,
                Name = $"{client.Name} - Main HQ",
                Address = client.Address ?? "100 Main St",
                City = client.City ?? "New York",
                ContactName = "Operations Manager",
                ContactPhone = client.Phone ?? "555-0000",
                Latitude = 40.7128m,
                Longitude = -74.0060m,
                IsDefault = true,
                IsActive = true
            });

            locations.Add(new ClientLocation
            {
                ClientId = client.Id,
                Name = $"{client.Name} - Distribution Hub",
                Address = $"500 Warehouse Ave",
                City = client.City ?? "New York",
                ContactName = "Warehouse Lead",
                ContactPhone = client.Mobile ?? "555-0001",
                Latitude = 40.7306m,
                Longitude = -73.9352m,
                IsDefault = false,
                IsActive = true
            });
        }

        await context.ClientLocations.AddRangeAsync(locations);
        await context.SaveChangesAsync();
    }

    private static async Task SeedOrdersAsync(CleanSampleDbContext context)
    {
        if (await context.Orders.AnyAsync()) return;

        var client1 = await context.Clients.FirstOrDefaultAsync();
        if (client1 == null) return;

        var orders = new List<Order>
        {
            new Order
            {
                OrderNumber = "ORD-2026-0001",
                ClientId = client1.Id,
                OrderDate = DateTime.UtcNow.AddDays(-5),
                RequiredDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
                Status = "Confirmed",
                Notes = "Priority delivery for Q3 office expansion"
            },
            new Order
            {
                OrderNumber = "ORD-2026-0002",
                ClientId = client1.Id,
                OrderDate = DateTime.UtcNow.AddDays(-2),
                RequiredDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(15)),
                Status = "Processing",
                Notes = "Standard equipment deployment"
            },
            new Order
            {
                OrderNumber = "ORD-2026-0003",
                ClientId = client1.Id,
                OrderDate = DateTime.UtcNow.AddDays(-1),
                RequiredDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7)),
                Status = "Pending",
                Notes = "Urgent spare units order"
            }
        };

        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync();
    }

    private static async Task SeedOrderLinesAsync(CleanSampleDbContext context)
    {
        if (await context.OrderLines.AnyAsync()) return;

        var orders = await context.Orders.ToListAsync();
        var variants = await context.ProductVariants.Take(3).ToListAsync();

        if (!orders.Any() || !variants.Any()) return;

        var orderLines = new List<OrderLine>();

        foreach (var order in orders)
        {
            foreach (var variant in variants)
            {
                orderLines.Add(new OrderLine
                {
                    OrderId = order.Id,
                    ProductVariantId = variant.Id,
                    Quantity = 5,
                    Notes = $"Line item for {variant.Code}"
                });
            }
        }

        await context.OrderLines.AddRangeAsync(orderLines);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPickRequestsAsync(CleanSampleDbContext context)
    {
        if (await context.PickRequests.AnyAsync()) return;

        var order = await context.Orders.FirstOrDefaultAsync();
        var client = await context.Clients.FirstOrDefaultAsync();
        var clientLocation = await context.ClientLocations.FirstOrDefaultAsync();
        var driver = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("driver"));
        var requester = await context.Users.FirstOrDefaultAsync(u => u.UserName == "admin");
        var vehicle = await context.Vehicles.FirstOrDefaultAsync();

        if (client == null) return;

        var pickRequests = new List<PickRequest>
        {
            new PickRequest
            {
                RequestNumber = "PR-2026-0001",
                OrderId = order?.Id,
                ClientId = client.Id,
                ClientLocationId = clientLocation?.Id,
                RequestedBy = requester?.Id,
                RequestDate = DateTime.UtcNow.AddDays(-3),
                ExecutionDate = DateTime.UtcNow.AddDays(1),
                Status = "Created",
                DestinationAddress = "100 Innovation Way, Suite 400",
                DestinationCity = "New York",
                Description = "Pick request for Order ORD-2026-0001 workstation batch",
                DriverId = driver?.Id,
                VehicleId = vehicle?.Id,
                Verified = false
            },
            new PickRequest
            {
                RequestNumber = "PR-2026-0002",
                OrderId = order?.Id,
                ClientId = client.Id,
                ClientLocationId = clientLocation?.Id,
                RequestedBy = requester?.Id,
                RequestDate = DateTime.UtcNow.AddDays(-2),
                ExecutionDate = DateTime.UtcNow.AddDays(2),
                Status = "Picking",
                DestinationAddress = "250 Freight Terminal Blvd",
                DestinationCity = "Chicago",
                Description = "Pick request for field assembly equipment",
                DriverId = driver?.Id,
                VehicleId = vehicle?.Id,
                Verified = false
            },
            new PickRequest
            {
                RequestNumber = "PR-2026-0003",
                OrderId = order?.Id,
                ClientId = client.Id,
                ClientLocationId = clientLocation?.Id,
                RequestedBy = requester?.Id,
                RequestDate = DateTime.UtcNow.AddDays(-1),
                ExecutionDate = DateTime.UtcNow.AddDays(3),
                Status = "Completed",
                DestinationAddress = "88 Industrial Parkway",
                DestinationCity = "Houston",
                Description = "Pick request for emergency maintenance replenishment",
                DriverId = driver?.Id,
                VehicleId = vehicle?.Id,
                Verified = true
            }
        };

        await context.PickRequests.AddRangeAsync(pickRequests);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPickRequestLinesAsync(CleanSampleDbContext context)
    {
        if (await context.PickRequestLines.AnyAsync()) return;

        var pickRequests = await context.PickRequests.ToListAsync();
        var variants = await context.ProductVariants.Take(2).ToListAsync();

        if (!pickRequests.Any() || !variants.Any()) return;

        var lines = new List<PickRequestLine>();

        foreach (var pr in pickRequests)
        {
            foreach (var variant in variants)
            {
                lines.Add(new PickRequestLine
                {
                    PickRequestId = pr.Id,
                    ProductVariantId = variant.Id,
                    Quantity = 4
                });
            }
        }

        await context.PickRequestLines.AddRangeAsync(lines);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPickRequestPartsAsync(CleanSampleDbContext context)
    {
        if (await context.PickRequestParts.AnyAsync()) return;

        var prLines = await context.PickRequestLines.Include(l => l.PickRequest).ToListAsync();
        var parts = await context.Parts.Take(3).ToListAsync();

        if (!prLines.Any() || !parts.Any()) return;

        var prParts = new List<PickRequestPart>();

        foreach (var line in prLines)
        {
            foreach (var part in parts)
            {
                prParts.Add(new PickRequestPart
                {
                    PickRequestId = line.PickRequestId,
                    PickRequestLineId = line.Id,
                    PartId = part.Id,
                    RequiredQuantity = line.Quantity * 2.0m,
                    PickedQuantity = line.Quantity * 2.0m,
                    Status = "Picked",
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await context.PickRequestParts.AddRangeAsync(prParts);
        await context.SaveChangesAsync();
    }

    private static async Task SeedPicksAsync(CleanSampleDbContext context)
    {
        if (await context.Picks.AnyAsync()) return;

        var prParts = await context.PickRequestParts.Take(5).ToListAsync();
        var picker = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("tech")) ?? await context.Users.FirstOrDefaultAsync();
        var driver = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("driver")) ?? await context.Users.FirstOrDefaultAsync();
        var vehicle = await context.Vehicles.FirstOrDefaultAsync();

        if (!prParts.Any()) return;

        var picks = new List<Pick>();
        int index = 1;

        foreach (var prPart in prParts)
        {
            picks.Add(new Pick
            {
                PickRequestId = prPart.PickRequestId,
                PickRequestPartId = prPart.Id,
                PartId = prPart.PartId,
                Barcode = $"PICK-BC-{index:D4}",
                Quantity = prPart.RequiredQuantity,
                PickedBy = picker?.Id,
                DriverId = driver?.Id,
                VehicleId = vehicle?.Id,
                PickDate = DateTime.UtcNow,
                Status = "Picked",
                Notes = $"Completed bin pick #{index}"
            });
            index++;
        }

        await context.Picks.AddRangeAsync(picks);
        await context.SaveChangesAsync();
    }

    private static async Task SeedVehicleLoadsAsync(CleanSampleDbContext context)
    {
        if (await context.VehicleLoads.AnyAsync()) return;

        var pickRequest = await context.PickRequests.FirstOrDefaultAsync();
        var vehicle = await context.Vehicles.FirstOrDefaultAsync();
        var driver = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("driver")) ?? await context.Users.FirstOrDefaultAsync();
        var supervisor = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("supervisor")) ?? await context.Users.FirstOrDefaultAsync();

        if (pickRequest == null || vehicle == null || driver == null) return;

        var loads = new List<VehicleLoad>
        {
            new VehicleLoad
            {
                PickRequestId = pickRequest.Id,
                VehicleId = vehicle.Id,
                DriverId = driver.Id,
                LoadDate = DateTime.UtcNow,
                Status = "Loaded",
                Verified = true,
                VerifiedBy = supervisor?.Id,
                VerifiedAt = DateTime.UtcNow,
                Notes = "Loaded pallets verified against Manifest"
            }
        };

        await context.VehicleLoads.AddRangeAsync(loads);
        await context.SaveChangesAsync();
    }

    private static async Task SeedVehicleLoadItemsAsync(CleanSampleDbContext context)
    {
        if (await context.VehicleLoadItems.AnyAsync()) return;

        var vehicleLoad = await context.VehicleLoads.FirstOrDefaultAsync();
        var picks = await context.Picks.Take(3).ToListAsync();

        if (vehicleLoad == null || !picks.Any()) return;

        var items = new List<VehicleLoadItem>();

        foreach (var pick in picks)
        {
            items.Add(new VehicleLoadItem
            {
                VehicleLoadId = vehicleLoad.Id,
                PickId = pick.Id,
                PartId = pick.PartId,
                Barcode = pick.Barcode,
                Quantity = pick.Quantity,
                LoadedAt = DateTime.UtcNow
            });
        }

        await context.VehicleLoadItems.AddRangeAsync(items);
        await context.SaveChangesAsync();
    }

    private static async Task SeedFieldJobsAsync(CleanSampleDbContext context)
    {
        if (await context.FieldJobs.AnyAsync()) return;

        var pickRequest = await context.PickRequests.FirstOrDefaultAsync();
        var client = await context.Clients.FirstOrDefaultAsync();
        var clientLocation = await context.ClientLocations.FirstOrDefaultAsync();
        var technician = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("tech")) ?? await context.Users.FirstOrDefaultAsync();
        var supervisor = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("supervisor")) ?? await context.Users.FirstOrDefaultAsync();

        if (pickRequest == null || client == null) return;

        var jobs = new List<FieldJob>
        {
            new FieldJob
            {
                JobNumber = "JOB-2026-0001",
                PickRequestId = pickRequest.Id,
                ClientId = client.Id,
                ClientLocationId = clientLocation?.Id,
                TechnicianId = technician?.Id,
                SupervisorId = supervisor?.Id,
                ScheduledDate = DateTime.UtcNow.AddDays(1),
                StartDate = DateTime.UtcNow.AddDays(1).AddHours(2),
                CompletionDate = null,
                Status = "InProgress",
                Verified = false,
                Notes = "On-site assembly and calibration for client main floor"
            },
            new FieldJob
            {
                JobNumber = "JOB-2026-0002",
                PickRequestId = pickRequest.Id,
                ClientId = client.Id,
                ClientLocationId = clientLocation?.Id,
                TechnicianId = technician?.Id,
                SupervisorId = supervisor?.Id,
                ScheduledDate = DateTime.UtcNow.AddDays(3),
                StartDate = null,
                CompletionDate = null,
                Status = "Scheduled",
                Verified = false,
                Notes = "Scheduled setup for auxiliary warehouse units"
            }
        };

        await context.FieldJobs.AddRangeAsync(jobs);
        await context.SaveChangesAsync();
    }

    private static async Task SeedFieldAssembliesAsync(CleanSampleDbContext context)
    {
        if (await context.FieldAssemblies.AnyAsync()) return;

        var job = await context.FieldJobs.FirstOrDefaultAsync();
        var variant = await context.ProductVariants.FirstOrDefaultAsync();
        var technician = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("tech")) ?? await context.Users.FirstOrDefaultAsync();
        var supervisor = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("supervisor")) ?? await context.Users.FirstOrDefaultAsync();

        if (job == null || variant == null) return;

        var assemblies = new List<FieldAssembly>
        {
            new FieldAssembly
            {
                FieldJobId = job.Id,
                ProductVariantId = variant.Id,
                ProductBarcode = "SN-ASM-2026-001",
                Quantity = 1,
                AssemblyDate = DateTime.UtcNow,
                Status = "Completed",
                TechnicianId = technician?.Id,
                SupervisorId = supervisor?.Id,
                Verified = true,
                VerifiedAt = DateTime.UtcNow,
                Notes = "Assembled and tested per checklist"
            },
            new FieldAssembly
            {
                FieldJobId = job.Id,
                ProductVariantId = variant.Id,
                ProductBarcode = "SN-ASM-2026-002",
                Quantity = 1,
                AssemblyDate = null,
                Status = "InProgress",
                TechnicianId = technician?.Id,
                SupervisorId = supervisor?.Id,
                Verified = false,
                Notes = "Pending final cable fastening"
            }
        };

        await context.FieldAssemblies.AddRangeAsync(assemblies);
        await context.SaveChangesAsync();
    }

    private static async Task SeedIssuesAsync(CleanSampleDbContext context)
    {
        if (await context.Issues.AnyAsync()) return;

        var pickRequest = await context.PickRequests.FirstOrDefaultAsync();
        var fieldJob = await context.FieldJobs.FirstOrDefaultAsync();
        var fieldAssembly = await context.FieldAssemblies.FirstOrDefaultAsync();
        var reporter = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("tech")) ?? await context.Users.FirstOrDefaultAsync();
        var resolver = await context.Users.FirstOrDefaultAsync(u => u.UserName.Contains("supervisor")) ?? await context.Users.FirstOrDefaultAsync();

        var issues = new List<Issue>
        {
            new Issue
            {
                PickRequestId = pickRequest?.Id,
                FieldJobId = fieldJob?.Id,
                FieldAssemblyId = fieldAssembly?.Id,
                IssueType = "Component Defect",
                Description = "RAM connector latch on chassis was slightly misaligned during casing assembly.",
                Severity = "Medium",
                Status = "Resolved",
                ReportedBy = reporter?.Id,
                ReportedAt = DateTime.UtcNow.AddDays(-1),
                ResolvedBy = resolver?.Id,
                ResolvedAt = DateTime.UtcNow,
                ResolutionNotes = "Replaced chassis latch bracket with spare unit. Passes quality check."
            },
            new Issue
            {
                PickRequestId = pickRequest?.Id,
                FieldJobId = fieldJob?.Id,
                FieldAssemblyId = null,
                IssueType = "Delivery Delay",
                Description = "Traffic congestion on highway caused 30-minute delay in pick request delivery.",
                Severity = "Low",
                Status = "Open",
                ReportedBy = reporter?.Id,
                ReportedAt = DateTime.UtcNow,
                ResolvedBy = null,
                ResolvedAt = null,
                ResolutionNotes = null
            }
        };

        await context.Issues.AddRangeAsync(issues);
        await context.SaveChangesAsync();
    }
}