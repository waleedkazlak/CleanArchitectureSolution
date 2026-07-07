# Clean Architecture Solution - Complete Overview

## 📋 Table of Contents

1. [Quick Start](#quick-start)
2. [Solution Structure](#solution-structure)
3. [Layer Details](#layer-details)
4. [API Documentation](#api-documentation)
5. [Design Patterns](#design-patterns)
6. [Development Guide](#development-guide)

---

## 🚀 Quick Start

### Build the Solution
```bash
cd C:\Users\SV\source\repos\CleanSample
dotnet build
```

### Run the Application
```bash
cd CleanSample.Presentation
dotnet run
```

### Access the API
- **Base URL:** `https://localhost:5001`
- **API Endpoints:** `https://localhost:5001/api/products`
- **Swagger UI:** `https://localhost:5001/swagger`

---

## 📁 Solution Structure

### Four Independent Projects

```
CleanArchitectureSolution.sln
├── CleanSample.Domain           (Domain Layer - Core Business Logic)
├── CleanSample.Application      (Application Layer - CQRS)
├── CleanSample.Infrastructure   (Infrastructure Layer - Data Access)
└── CleanSample.Presentation     (Presentation Layer - API)
```

### Project Relationships

```
Presentation (API)
    │
    ├── depends on ──→ Application (CQRS)
    │                     │
    │                     └─→ depends on ──→ Domain (Business Logic)
    │
    └── depends on ──→ Infrastructure (Data Access)
                           │
                           └─→ depends on ──→ Domain (Business Logic)
```

---

## 📦 Layer Details

### 1️⃣ Domain Layer (CleanSample.Domain)
**Core business logic - completely independent**

**Files:**
- `Entities/BaseEntity.cs` - Base entity with audit fields
- `Entities/Product.cs` - Product domain entity
- `Interfaces/IProductRepository.cs` - Repository contract
- `Interfaces/IUnitOfWork.cs` - Unit of Work contract

**Characteristics:**
- ✅ Zero external dependencies
- ✅ Pure business logic
- ✅ Highly reusable
- ✅ Framework-independent

**NuGet Packages:** None

---

### 2️⃣ Application Layer (CleanSample.Application)
**CQRS pattern implementation - business operations**

**Files:**
- `Commands/Product/CreateProductCommand.cs` - Write operation
- `Commands/Product/CreateProductCommandHandler.cs` - Write handler
- `Commands/Product/UpdateProductCommand.cs` - Write operation
- `Commands/Product/UpdateProductCommandHandler.cs` - Write handler
- `Commands/Product/DeleteProductCommand.cs` - Write operation
- `Commands/Product/DeleteProductCommandHandler.cs` - Write handler
- `Queries/Product/GetProductByIdQuery.cs` - Read operation
- `Queries/Product/GetProductByIdQueryHandler.cs` - Read handler
- `Queries/Product/GetAllProductsQuery.cs` - Read operation
- `Queries/Product/GetAllProductsQueryHandler.cs` - Read handler
- `DTOs/ProductDto.cs` - Data transfer object
- `ApplicationServiceCollectionExtensions.cs` - DI configuration

**CQRS Separation:**
```
Write Operations (Commands)          Read Operations (Queries)
├── CreateProductCommand              ├── GetProductByIdQuery
├── UpdateProductCommand              └── GetAllProductsQuery
└── DeleteProductCommand
```

**Characteristics:**
- ✅ Implements CQRS pattern
- ✅ Depends only on Domain
- ✅ MediatR for request/response
- ✅ DTOs for API contracts

**NuGet Packages:**
- MediatR 12.2.0

---

### 3️⃣ Infrastructure Layer (CleanSample.Infrastructure)
**Data access and external services**

**Files:**
- `Persistence/ProductRepository.cs` - Repository implementation (in-memory)
- `Persistence/UnitOfWork.cs` - Unit of Work implementation
- `InfrastructureServiceCollectionExtensions.cs` - DI configuration

**Current Implementation:**
- In-memory storage (static List<Product>)
- Ready for database integration

**Characteristics:**
- ✅ Implements domain interfaces
- ✅ Depends only on Domain
- ✅ Handles all data operations
- ✅ Easy to replace with EF Core

**NuGet Packages:** None

---

### 4️⃣ Presentation Layer (CleanSample.Presentation)
**RESTful API - HTTP request handling**

**Files:**
- `Controllers/ProductsController.cs` - API endpoints
- `Program.cs` - Application entry point & DI setup
- `appsettings.json` - Configuration
- `appsettings.Development.json` - Dev configuration

**API Endpoints:**
```
GET    /api/products              - Get all products
GET    /api/products/{id}         - Get single product
POST   /api/products              - Create product
PUT    /api/products/{id}         - Update product
DELETE /api/products/{id}         - Delete product
```

**Characteristics:**
- ✅ Thin controller layer
- ✅ Delegates to application via MediatR
- ✅ Swagger/OpenAPI support
- ✅ Proper HTTP status codes

**NuGet Packages:**
- MediatR.Extensions.Microsoft.DependencyInjection 11.1.0
- Swashbuckle.AspNetCore 7.0.0

---

## 🔌 API Documentation

### Base URL
```
https://localhost:5001/api/products
```

### Endpoints

#### 1. Get All Products
```http
GET /api/products
Accept: application/json
```

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "description": "High-performance laptop",
    "price": 999.99,
    "stock": 10,
    "isActive": true,
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": null
  }
]
```

#### 2. Get Product by ID
```http
GET /api/products/1
Accept: application/json
```

**Response:** `200 OK`
```json
{
  "id": 1,
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 999.99,
  "stock": 10,
  "isActive": true,
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": null
}
```

**Response:** `404 Not Found` (if product doesn't exist)

#### 3. Create Product
```http
POST /api/products
Content-Type: application/json

{
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 999.99,
  "stock": 10
}
```

**Response:** `201 Created`
```json
1
```

**Response Header:**
```
Location: /api/products/1
```

#### 4. Update Product
```http
PUT /api/products/1
Content-Type: application/json

{
  "id": 1,
  "name": "Laptop Pro",
  "description": "Updated description",
  "price": 1299.99,
  "stock": 8,
  "isActive": true
}
```

**Response:** `204 No Content`

**Response:** `404 Not Found` (if product doesn't exist)

#### 5. Delete Product
```http
DELETE /api/products/1
```

**Response:** `204 No Content`

**Response:** `404 Not Found` (if product doesn't exist)

---

## 🎨 Design Patterns

### 1. Clean Architecture
- **Layer separation:** Domain → Application → Infrastructure, Presentation
- **Dependency inversion:** High-level modules depend on abstractions
- **Independent testability:** Each layer can be tested independently

### 2. CQRS (Command Query Responsibility Segregation)
- **Commands:** CreateProductCommand, UpdateProductCommand, DeleteProductCommand
- **Queries:** GetProductByIdQuery, GetAllProductsQuery
- **Benefits:** Clear intent, independent scaling, easier testing

### 3. Repository Pattern
- **Abstraction:** IProductRepository interface
- **Implementation:** ProductRepository (in-memory)
- **Benefits:** Loose coupling, easy to test, easy to swap

### 4. Unit of Work Pattern
- **Coordination:** IUnitOfWork coordinates repositories
- **Benefits:** Transaction management, atomic operations

### 5. Mediator Pattern
- **Library:** MediatR
- **Usage:** Controllers use IMediator to send commands/queries
- **Benefits:** Decoupling, extensibility

### 6. Dependency Injection
- **Setup:** Extension methods (AddApplication, AddInfrastructure)
- **Benefits:** Loose coupling, testability

### 7. Data Transfer Objects (DTOs)
- **Purpose:** ProductDto separates API contracts from domain models
- **Benefits:** API stability, domain independence

---

## 🛠️ Development Guide

### Adding a New Entity

#### 1. Create Domain Entity
**File:** `CleanSample.Domain/Entities/YourEntity.cs`
```csharp
public class YourEntity : BaseEntity
{
    public string Name { get; set; } = null!;
    // Add properties...
}
```

#### 2. Create Repository Interface
**File:** `CleanSample.Domain/Interfaces/IYourEntityRepository.cs`
```csharp
public interface IYourEntityRepository
{
    Task<YourEntity?> GetByIdAsync(int id);
    Task<IEnumerable<YourEntity>> GetAllAsync();
    Task<int> AddAsync(YourEntity entity);
    Task UpdateAsync(YourEntity entity);
    Task DeleteAsync(int id);
}
```

#### 3. Create Application Commands
**File:** `CleanSample.Application/Commands/YourEntity/CreateYourEntityCommand.cs`
```csharp
public class CreateYourEntityCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    // Add properties...
}

public class CreateYourEntityCommandHandler : IRequestHandler<CreateYourEntityCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public async Task<int> Handle(CreateYourEntityCommand request, CancellationToken cancellationToken)
    {
        var entity = new YourEntity { Name = request.Name };
        var id = await _unitOfWork.YourEntities.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return id;
    }
}
```

#### 4. Create Application Queries
Similar to commands, create GetByIdQuery and GetAllQuery handlers.

#### 5. Create Infrastructure Repository
**File:** `CleanSample.Infrastructure/Persistence/YourEntityRepository.cs`
```csharp
public class YourEntityRepository : IYourEntityRepository
{
    private static readonly List<YourEntity> Entities = new();
    private static int _nextId = 1;

    // Implement interface methods...
}
```

#### 6. Update Unit of Work
```csharp
public interface IUnitOfWork
{
    IProductRepository Products { get; }
    IYourEntityRepository YourEntities { get; }  // Add this
    Task<int> SaveChangesAsync();
}
```

#### 7. Create API Controller
**File:** `CleanSample.Presentation/Controllers/YourEntitiesController.cs`
```csharp
[ApiController]
[Route("api/[controller]")]
public class YourEntitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public YourEntitiesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateYourEntityCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    // Add other endpoint methods...
}
```

---

## 📊 Project Statistics

| Aspect | Count |
|--------|-------|
| Projects | 4 |
| Domain Entities | 1 (Product) |
| Repositories | 1 (Product) |
| Commands | 3 (Create, Update, Delete) |
| Queries | 2 (GetById, GetAll) |
| API Endpoints | 5 |
| NuGet Packages | 3 |
| Classes | 20+ |
| Total Lines of Code | 500+ |

---

## 🧪 Testing Strategy

### Unit Tests (Domain Layer)
```csharp
[Fact]
public void Product_Should_Initialize_With_Active_Status()
{
    var product = new Product { Name = "Test" };
    Assert.True(product.IsActive);
}
```

### Integration Tests (Application Layer)
```csharp
[Fact]
public async Task CreateProductCommand_Should_Return_ProductId()
{
    var handler = new CreateProductCommandHandler(_unitOfWork);
    var command = new CreateProductCommand { Name = "Test", Price = 99.99m, Stock = 10 };

    var result = await handler.Handle(command, CancellationToken.None);

    Assert.True(result > 0);
}
```

### API Tests (Presentation Layer)
```csharp
[Fact]
public async Task GetProducts_Should_Return_Ok()
{
    var response = await _client.GetAsync("/api/products");
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
}
```

---

## 🚢 Deployment

### Docker Support
Create `Dockerfile` for containerization:
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CleanSample.Presentation.dll"]
```

### CI/CD Pipeline
Create `.github/workflows/build.yml` for automated testing and deployment.

---

## 📚 Resources

- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [Repository Pattern](https://martinfowler.com/eaaCatalog/repository.html)
- [Unit of Work Pattern](https://martinfowler.com/eaaCatalog/unitOfWork.html)

---

## ✅ Build Status

All projects build successfully with no errors.

```
CleanSample.Domain          ✓ Build Successful
CleanSample.Application     ✓ Build Successful
CleanSample.Infrastructure  ✓ Build Successful
CleanSample.Presentation    ✓ Build Successful
```

---

## 📞 Support

For questions or issues:
1. Check the documentation in each project
2. Review the MULTI_PROJECT_GUIDE.md for architecture details
3. Examine the example implementations in ProductsController

