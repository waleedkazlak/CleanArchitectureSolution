using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Product;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedResultDto<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllProductsQueryHandler> _logger;

    public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllProductsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling GetAllProductsQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}, MinPrice: {MinPrice}, MaxPrice: {MaxPrice}",
            request.PageNumber, request.PageSize, request.SearchTerm, request.MinPrice, request.MaxPrice);

        // Validate pagination parameters
        var pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
        var pageSize = request.PageSize > 0 && request.PageSize <= 100 ? request.PageSize : 10;

        // Get all products from repository
        var products = await _unitOfWork.Products.GetAllAsync();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTermLower = request.SearchTerm.ToLower();
            products = products.Where(p =>
                p.Name.ToLower().Contains(searchTermLower) ||
                p.Description.ToLower().Contains(searchTermLower)
            ).ToList();

            _logger.LogInformation("Applied search filter '{SearchTerm}', found {Count} products", request.SearchTerm, products.Count());
        }

        // Apply price filter
        if (request.MinPrice.HasValue)
        {
            products = products.Where(p => p.Price >= request.MinPrice.Value).ToList();
            _logger.LogInformation("Applied minimum price filter {MinPrice}", request.MinPrice);
        }

        if (request.MaxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= request.MaxPrice.Value).ToList();
            _logger.LogInformation("Applied maximum price filter {MaxPrice}", request.MaxPrice);
        }

        // Apply active status filter
        if (request.IsActive.HasValue)
        {
            products = products.Where(p => p.IsActive == request.IsActive.Value).ToList();
            _logger.LogInformation("Applied active status filter {IsActive}", request.IsActive);
        }

        // Apply stock filter
        if (request.MinStock.HasValue)
        {
            products = products.Where(p => p.Stock >= request.MinStock.Value).ToList();
            _logger.LogInformation("Applied minimum stock filter {MinStock}", request.MinStock);
        }

        // Apply sorting
        products = ApplySort(products.ToList(), request.SortBy, request.SortDirection);

        var totalCount = products.Count();

        // Apply pagination
        var paginatedProducts = products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var productDtos = paginatedProducts.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();

        _logger.LogInformation(
            "Query completed - Returned {Count} products from {TotalCount} total on page {PageNumber}",
            paginatedProducts.Count, totalCount, pageNumber);

        return new PaginatedResultDto<ProductDto>
        {
            Items = productDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Product> ApplySort(List<Domain.Entities.Product> products, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "name" => isDescending
                ? products.OrderByDescending(p => p.Name).ToList()
                : products.OrderBy(p => p.Name).ToList(),

            "price" => isDescending
                ? products.OrderByDescending(p => p.Price).ToList()
                : products.OrderBy(p => p.Price).ToList(),

            "stock" => isDescending
                ? products.OrderByDescending(p => p.Stock).ToList()
                : products.OrderBy(p => p.Stock).ToList(),

            "createdat" => isDescending
                ? products.OrderByDescending(p => p.CreatedAt).ToList()
                : products.OrderBy(p => p.CreatedAt).ToList(),

            _ => products.OrderByDescending(p => p.CreatedAt).ToList()
        };
    }
}
