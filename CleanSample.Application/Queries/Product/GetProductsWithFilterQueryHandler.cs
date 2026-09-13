using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Product;

/// <summary>
/// Handler for GetProductsWithFilterQuery
/// </summary>
public class GetProductsWithFilterQueryHandler : IRequestHandler<GetProductsWithFilterQuery, PaginatedResultDto<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProductsWithFilterQueryHandler> _logger;

    public GetProductsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<ProductDto>> Handle(GetProductsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetProductsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        // Validate pagination parameters
        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        // Get all products from repository
        var products = await _unitOfWork.Products.GetAllAsync();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            products = products.Where(p =>
                p.NameEn.ToLower().Contains(searchTermLower) ||
                (p.NameAr != null && p.NameAr.ToLower().Contains(searchTermLower)) ||
                (p.Barcode != null && p.Barcode.ToLower().Contains(searchTermLower)) ||
                (p.DescriptionEn != null && p.DescriptionEn.ToLower().Contains(searchTermLower)) ||
                (p.DescriptionAr != null && p.DescriptionAr.ToLower().Contains(searchTermLower))
            ).ToList();

            _logger.LogInformation("Applied search filter '{SearchTerm}', found {Count} products", filter.SearchTerm, products.Count());
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var nameLower = filter.Name.ToLower();
            products = products.Where(p => 
                p.NameEn.ToLower().Contains(nameLower) || 
                (p.NameAr != null && p.NameAr.ToLower().Contains(nameLower))
            ).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Barcode))
        {
            products = products.Where(p => p.Barcode != null && p.Barcode.Contains(filter.Barcode, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Apply active status filter
        if (filter.IsActive.HasValue)
        {
            products = products.Where(p => p.IsActive == filter.IsActive.Value).ToList();
            _logger.LogInformation("Applied active status filter {IsActive}", filter.IsActive);
        }

        // Apply category filter
        if (filter.CategoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == filter.CategoryId.Value).ToList();
            _logger.LogInformation("Applied category filter {CategoryId}", filter.CategoryId);
        }

        if (filter.ColorId.HasValue)
        {
            products = products.Where(p => p.ColorId == filter.ColorId.Value).ToList();
        }

        if (filter.MaterialId.HasValue)
        {
            products = products.Where(p => p.MaterialId == filter.MaterialId.Value).ToList();
        }

        if (filter.DesignId.HasValue)
        {
            products = products.Where(p => p.DesignId == filter.DesignId.Value).ToList();
        }

        // Apply sorting
        products = ApplySort(products.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = products.Count();

        // Apply pagination
        var paginatedProducts = products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var productDtos = paginatedProducts.Select(p => new ProductDto
        {
            Id = p.Id,
            CategoryId = p.CategoryId,
            CategoryName = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.Category?.NameEn, p.Category?.NameAr),
            ColorId = p.ColorId,
            ColorName = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.Color?.NameEn, p.Color?.NameAr),
            MaterialId = p.MaterialId,
            MaterialName = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.Material?.NameEn, p.Material?.NameAr),
            DesignId = p.DesignId,
            DesignName = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.Design?.NameEn, p.Design?.NameAr),
            Name = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.NameEn, p.NameAr) ?? p.NameEn,
            NameEn = p.NameEn,
            NameAr = p.NameAr,
            Barcode = p.Barcode,
            Description = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.DescriptionEn, p.DescriptionAr),
            DescriptionEn = p.DescriptionEn,
            DescriptionAr = p.DescriptionAr,
            PictureUrl = p.PictureUrl,
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

            "barcode" => isDescending
                ? products.OrderByDescending(p => p.Barcode).ToList()
                : products.OrderBy(p => p.Barcode).ToList(),

            "categoryid" => isDescending
                ? products.OrderByDescending(p => p.CategoryId).ToList()
                : products.OrderBy(p => p.CategoryId).ToList(),

            "createdat" => isDescending
                ? products.OrderByDescending(p => p.CreatedAt).ToList()
                : products.OrderBy(p => p.CreatedAt).ToList(),

            _ => products.OrderByDescending(p => p.CreatedAt).ToList()
        };
    }
}