# Visual Architecture Reference Guide

## 📐 Complete Solution Architecture

```
╔══════════════════════════════════════════════════════════════════════════════╗
║                        CLEAN ARCHITECTURE SOLUTION                           ║
║                      (4 Separate Projects / 4 Layers)                        ║
╚══════════════════════════════════════════════════════════════════════════════╝

┌──────────────────────────────────────────────────────────────────────────────┐
│  PRESENTATION LAYER - CleanSample.Presentation                               │
│  (ASP.NET Core Web API)                                                      │
│                                                                              │
│  ┌────────────────────────────────────────────────────────────────────────┐ │
│  │ ProductsController                                                     │ │
│  ├─ GET    /api/products           → GetAllProductsQuery                 │ │
│  ├─ GET    /api/products/{id}      → GetProductByIdQuery                │ │
│  ├─ POST   /api/products           → CreateProductCommand                │ │
│  ├─ PUT    /api/products/{id}      → UpdateProductCommand                │ │
│  └─ DELETE /api/products/{id}      → DeleteProductCommand                │ │
│  └────────────────────────────────────────────────────────────────────────┘ │
│                                                                              │
│  NuGet: MediatR.Extensions, Swashbuckle.AspNetCore                          │
└──────────────────────────────────┬───────────────────────────────────────────┘
                                   │ depends on
                        ┌──────────┴──────────┐
                        │                     │
┌───────────────────────▼─────────────────┐  │  ┌──────────────────────────────┐
│ APPLICATION LAYER                       │  │  │ INFRASTRUCTURE LAYER        │
│ CleanSample.Application (CQRS)         │  │  │ CleanSample.Infrastructure  │
│                                         │  │  │                            │
│ ┌─────────────────────────────────────┐│  │  │ ┌──────────────────────────┐│
│ │ Commands (Write Operations)         ││  │  │ │ Repositories            ││
│ ├─ CreateProductCommand               ││  │  │ ├─ ProductRepository      ││
│ ├─ UpdateProductCommand               ││  │  │ └─ (In-Memory Storage)   ││
│ ├─ DeleteProductCommand               ││  │  │                          ││
│ └─────────────────────────────────────┘│  │  │ ┌──────────────────────────┐│
│                                         │  │  │ │ Unit of Work            ││
│ ┌─────────────────────────────────────┐│  │  │ ├─ Coordinates repos     ││
│ │ Queries (Read Operations)           ││  │  │ └─ Transaction mgmt      ││
│ ├─ GetProductByIdQuery                ││  │  │                          ││
│ ├─ GetAllProductsQuery                ││  │  │ NuGet: None              ││
│ └─────────────────────────────────────┘│  │  │ (Ready for EF Core)      ││
│                                         │  │  │                          ││
│ ┌─────────────────────────────────────┐│  │  └──────────────────────────┘│
│ │ DTOs                                ││  │  │                            │
│ ├─ ProductDto                         ││  │  └────────────────────────────┘
│ └─────────────────────────────────────┘│  │
│                                         │  │
│ NuGet: MediatR                          │  │
└─────────────────────────────────────────┴──┘
                                   │
                                   │ depends on
                                   │
┌──────────────────────────────────▼───────────────────────────────────────────┐
│ DOMAIN LAYER - CleanSample.Domain                                             │
│ (Pure Business Logic - Zero Dependencies)                                    │
│                                                                              │
│ ┌────────────────────────────────────────────────────────────────────────┐ │
│ │ Entities                                                               │ │
│ ├─ BaseEntity (abstract)                                                 │ │
│ │  └─ Properties: Id, CreatedAt, UpdatedAt                             │ │
│ └─ Product : BaseEntity                                                 │ │
│  │  └─ Properties: Name, Description, Price, Stock, IsActive          │ │
│ └────────────────────────────────────────────────────────────────────────┘ │
│                                                                              │
│ ┌────────────────────────────────────────────────────────────────────────┐ │
│ │ Interfaces (Contracts)                                                 │ │
│ ├─ IProductRepository                                                    │ │
│ │  ├─ GetByIdAsync(id)                                                  │ │
│ │  ├─ GetAllAsync()                                                     │ │
│ │  ├─ AddAsync(product)                                                 │ │
│ │  ├─ UpdateAsync(product)                                              │ │
│ │  └─ DeleteAsync(id)                                                   │ │
│ └─ IUnitOfWork                                                           │ │
│  ├─ Products : IProductRepository                                       │ │
│  └─ SaveChangesAsync()                                                  │ │
│ └────────────────────────────────────────────────────────────────────────┘ │
│                                                                              │
│ NuGet: None (Pure .NET Core)                                               │
└──────────────────────────────────────────────────────────────────────────────┘
```

---

## 🔄 Request Flow Diagram

```
HTTP Client Request
        │
        ▼
┌──────────────────────┐
│ API Endpoint         │
│ POST /api/products   │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ ProductsController   │
│ Create() method      │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────────────────────────┐
│ MediatR IMediator                        │
│ _mediator.Send(CreateProductCommand)     │
└──────────┬───────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────┐
│ CreateProductCommandHandler              │
│ Handle(CreateProductCommand)             │
└──────────┬───────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────┐
│ Create Product Entity                    │
│ Product product = new Product { ... }    │
└──────────┬───────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────┐
│ Unit of Work                             │
│ _unitOfWork.Products.AddAsync(product)   │
└──────────┬───────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────┐
│ Product Repository                       │
│ ProductRepository.AddAsync(product)      │
│ (In-Memory Storage)                      │
└──────────┬───────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────┐
│ Save Changes                             │
│ _unitOfWork.SaveChangesAsync()           │
└──────────┬───────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────┐
│ Return Product ID                        │
│ return productId;                        │
└──────────┬───────────────────────────────┘
           │
           ▼
┌──────────────────────────────────────────┐
│ API Response                             │
│ 201 Created                              │
│ Location: /api/products/1                │
│ Body: 1 (Product ID)                     │
└──────────────────────────────────────────┘
           │
           ▼
HTTP Client receives response
```

---

## 📊 Data Flow - CQRS Pattern

### Write Side (Commands)
```
Client Request
    │
    ├─ CreateProductCommand
    │   └─ CreateProductCommandHandler
    │       ├─ Validate
    │       ├─ Create Entity
    │       ├─ Persist
    │       └─ Return ID
    │
    ├─ UpdateProductCommand
    │   └─ UpdateProductCommandHandler
    │       ├─ Fetch Entity
    │       ├─ Update Properties
    │       ├─ Persist
    │       └─ Return bool
    │
    └─ DeleteProductCommand
        └─ DeleteProductCommandHandler
            ├─ Fetch Entity
            ├─ Delete
            └─ Return bool
```

### Read Side (Queries)
```
Client Request
    │
    ├─ GetProductByIdQuery
    │   └─ GetProductByIdQueryHandler
    │       ├─ Fetch from Repository
    │       ├─ Map to DTO
    │       └─ Return ProductDto
    │
    └─ GetAllProductsQuery
        └─ GetAllProductsQueryHandler
            ├─ Fetch All from Repository
            ├─ Map to DTOs
            └─ Return IEnumerable<ProductDto>
```

---

## 🏗️ Project Structure Visualization

```
CleanSample (Root Directory)
│
├── [📦 CleanSample.Domain]
│   ├── Entities/
│   │   ├── BaseEntity.cs
│   │   └── Product.cs
│   └── Interfaces/
│       ├── IProductRepository.cs
│       └── IUnitOfWork.cs
│
├── [📦 CleanSample.Application]
│   ├── Commands/
│   │   └── Product/
│   │       ├── CreateProductCommand.cs
│   │       ├── CreateProductCommandHandler.cs
│   │       ├── UpdateProductCommand.cs
│   │       ├── UpdateProductCommandHandler.cs
│   │       ├── DeleteProductCommand.cs
│   │       └── DeleteProductCommandHandler.cs
│   ├── Queries/
│   │   └── Product/
│   │       ├── GetProductByIdQuery.cs
│   │       ├── GetProductByIdQueryHandler.cs
│   │       ├── GetAllProductsQuery.cs
│   │       └── GetAllProductsQueryHandler.cs
│   ├── DTOs/
│   │   └── ProductDto.cs
│   └── ApplicationServiceCollectionExtensions.cs
│
├── [📦 CleanSample.Infrastructure]
│   ├── Persistence/
│   │   ├── ProductRepository.cs
│   │   └── UnitOfWork.cs
│   └── InfrastructureServiceCollectionExtensions.cs
│
├── [📦 CleanSample.Presentation]
│   ├── Controllers/
│   │   └── ProductsController.cs
│   ├── Program.cs
│   ├── appsettings.json
│   └── appsettings.Development.json
│
└── [📋 CleanArchitectureSolution.sln]
```

---

## 🔌 API Endpoint Mapping

```
┌─────────────────────────────────────────────────────────────────┐
│                     API ENDPOINTS                              │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  GET /api/products                                             │
│  ├─ Controller: ProductsController.GetAll()                   │
│  ├─ Query: GetAllProductsQuery                                │
│  ├─ Handler: GetAllProductsQueryHandler                       │
│  └─ Response: IEnumerable<ProductDto>                         │
│                                                                 │
│  GET /api/products/{id}                                        │
│  ├─ Controller: ProductsController.GetById(id)                │
│  ├─ Query: GetProductByIdQuery                                │
│  ├─ Handler: GetProductByIdQueryHandler                       │
│  └─ Response: ProductDto or 404                               │
│                                                                 │
│  POST /api/products                                            │
│  ├─ Controller: ProductsController.Create(command)            │
│  ├─ Command: CreateProductCommand                             │
│  ├─ Handler: CreateProductCommandHandler                      │
│  └─ Response: 201 Created with ID                             │
│                                                                 │
│  PUT /api/products/{id}                                        │
│  ├─ Controller: ProductsController.Update(id, command)        │
│  ├─ Command: UpdateProductCommand                             │
│  ├─ Handler: UpdateProductCommandHandler                      │
│  └─ Response: 204 No Content or 404                           │
│                                                                 │
│  DELETE /api/products/{id}                                     │
│  ├─ Controller: ProductsController.Delete(id)                 │
│  ├─ Command: DeleteProductCommand                             │
│  ├─ Handler: DeleteProductCommandHandler                      │
│  └─ Response: 204 No Content or 404                           │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

---

## 📈 Dependency Hierarchy

```
Level 4 (Outermost)
┌─────────────────────────────────┐
│  Presentation                   │
│  (CleanSample.Presentation)     │
│  - Controllers                  │
│  - HTTP Handling                │
└────────┬────────────────────────┘
         │
         ├─ depends on ─────┐
         │                  │
Level 3  │    ┌─────────────▼──────────────────┐      ┌─────────────────────────────┐
         │    │ Application                    │      │ Infrastructure              │
         │    │ (CleanSample.Application)      │      │ (CleanSample.Infrastructure)│
         │    │ - CQRS Commands                │      │ - Repositories              │
         │    │ - CQRS Queries                 │      │ - Unit of Work              │
         │    │ - DTOs                         │      │ - Data Access               │
         │    └─────────┬──────────────────────┘      └────────┬────────────────────┘
         │              │                                      │
         │              └──────────┬───────────────────────────┘
         │                         │
         │              ┌──────────▼───────────┐
         │              │ depends on           │
         │              │                      │
Level 2  │              │   ┌─────────────────▼────────────────┐
         │              │   │ Domain                           │
         │              │   │ (CleanSample.Domain)             │
         │              │   │ - Entities                       │
         │              │   │ - Interfaces                     │
         │              │   │ - Pure Business Logic            │
         │              │   │ - ZERO Dependencies              │
         │              │   └──────────────────────────────────┘
         │              │
Level 1  └──────────────┘

Rule: Dependencies ALWAYS flow INWARD toward Domain
      Domain has NO dependencies on other layers
```

---

## 🔐 Interface Contracts

```
┌─────────────────────────────────────────────────────────┐
│           DOMAIN LAYER INTERFACES                       │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  IProductRepository                                    │
│  ├─ Task<Product?> GetByIdAsync(int id)              │
│  ├─ Task<IEnumerable<Product>> GetAllAsync()         │
│  ├─ Task<int> AddAsync(Product product)              │
│  ├─ Task UpdateAsync(Product product)                │
│  └─ Task DeleteAsync(int id)                         │
│                                                         │
│  IUnitOfWork : IDisposable                            │
│  ├─ IProductRepository Products { get; }             │
│  └─ Task<int> SaveChangesAsync()                     │
│                                                         │
└─────────────────────────────────────────────────────────┘

        ▲                          ▲
        │ Implemented by           │ Implemented by
        │                          │
┌───────┴───────────────┐  ┌──────┴──────────────────┐
│ ProductRepository     │  │ UnitOfWork              │
│ (Infrastructure)      │  │ (Infrastructure)        │
└───────────────────────┘  └─────────────────────────┘
```

---

## 📝 CQRS Query/Command Structure

```
┌──────────────────────────────────────────────────────────┐
│                    CQRS Pattern                         │
├──────────────────┬──────────────────────────────────────┤
│    COMMANDS      │          QUERIES                     │
│  (Write)         │          (Read)                      │
├──────────────────┼──────────────────────────────────────┤
│                  │                                      │
│ CreateProductCmd │ GetProductByIdQuery                │
│  └─ Name         │  └─ Id                            │
│  └─ Desc         │     └─ Returns: ProductDto|null   │
│  └─ Price        │                                    │
│  └─ Stock        │ GetAllProductsQuery                │
│                  │  └─ (no parameters)                │
│ UpdateProductCmd │     └─ Returns: IEnum<ProductDto> │
│  └─ Id           │                                    │
│  └─ Name         │                                    │
│  └─ Desc         │                                    │
│  └─ Price        │                                    │
│  └─ Stock        │                                    │
│  └─ IsActive     │                                    │
│                  │                                    │
│ DeleteProductCmd │                                    │
│  └─ Id           │                                    │
│                  │                                    │
└──────────────────┴──────────────────────────────────────┘

Benefits:
✓ Clear separation of read/write
✓ Different optimization strategies
✓ Independent scaling
✓ Explicit intent in code
```

---

## 🎯 Development Workflow

```
┌─────────────────────────────────────────────────────────┐
│           ADDING A NEW FEATURE                         │
├─────────────────────────────────────────────────────────┤
│                                                         │
│ 1️⃣  Domain Layer                                       │
│    └─ Create Entity (inherits BaseEntity)             │
│    └─ Create Repository Interface (I*Repository)      │
│    └─ Create Unit of Work Interface update            │
│                                                         │
│ 2️⃣  Application Layer                                 │
│    └─ Create Command(s) for write operations          │
│    └─ Create CommandHandler(s)                        │
│    └─ Create Query/Queries for read operations        │
│    └─ Create QueryHandler(s)                          │
│    └─ Create DTO for API response                     │
│                                                         │
│ 3️⃣  Infrastructure Layer                              │
│    └─ Create Repository implementation                │
│    └─ Update Unit of Work implementation              │
│    └─ Register in DI                                  │
│                                                         │
│ 4️⃣  Presentation Layer                                │
│    └─ Create API Controller                           │
│    └─ Define endpoints (GET, POST, PUT, DELETE)       │
│    └─ Inject IMediator                                │
│    └─ Send commands/queries via mediator              │
│                                                         │
└─────────────────────────────────────────────────────────┘
```

---

## 📊 Technology Stack

```
┌────────────────────────────────────────────────────────┐
│           TECHNOLOGY STACK                             │
├────────────────────────────────────────────────────────┤
│                                                        │
│  Platform:                                             │
│  └─ .NET 10.0                                         │
│                                                        │
│  Web Framework:                                        │
│  └─ ASP.NET Core (Web API)                           │
│                                                        │
│  CQRS/Mediator:                                        │
│  └─ MediatR 12.2.0                                    │
│                                                        │
│  API Documentation:                                    │
│  └─ Swagger / OpenAPI (Swashbuckle.AspNetCore)       │
│                                                        │
│  Dependency Injection:                                 │
│  └─ Microsoft.Extensions.DependencyInjection          │
│                                                        │
│  Ready for Integration:                                │
│  └─ Entity Framework Core (EF7/EF8)                  │
│  └─ Serilog (Logging)                                │
│  └─ FluentValidation (Validation)                    │
│                                                        │
└────────────────────────────────────────────────────────┘
```

This visual reference guide provides a complete overview of the Clean Architecture solution and how all components interact!
