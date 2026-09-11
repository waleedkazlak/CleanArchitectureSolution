using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Product;

/// <summary>
/// Query for fetching all products with pagination and search support
/// </summary>
public class GetAllProductsQuery : IRequest<PaginatedResultDto<ProductDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public int? ColorId { get; set; }
    public int? MaterialId { get; set; }
    public int? DesignId { get; set; }
    public bool? IsActive { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortDirection { get; set; } = "desc";
}
