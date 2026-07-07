using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Product;

/// <summary>
/// Query for fetching all products with pagination and search support
/// </summary>
public class GetAllProductsQuery : IRequest<PaginatedResultDto<ProductDto>>
{
    /// <summary>
    /// Page number (1-based), defaults to 1
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of items per page, defaults to 10
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Search term to filter products by name or description
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Minimum price filter
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Maximum price filter
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Filter by active status
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Minimum stock filter
    /// </summary>
    public int? MinStock { get; set; }

    /// <summary>
    /// Sort by field (Name, Price, Stock, CreatedAt)
    /// </summary>
    public string? SortBy { get; set; } = "CreatedAt";

    /// <summary>
    /// Sort direction (asc, desc)
    /// </summary>
    public string? SortDirection { get; set; } = "desc";

}
