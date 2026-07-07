using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Product;

/// <summary>
/// Query for fetching products with advanced filtering
/// </summary>
public class GetProductsWithFilterQuery : IRequest<PaginatedResultDto<ProductDto>>
{
    /// <summary>
    /// Product search filter
    /// </summary>
    public ProductSearchFilterDto Filter { get; set; } = new();

    public GetProductsWithFilterQuery()
    {
    }

    public GetProductsWithFilterQuery(ProductSearchFilterDto filter)
    {
        Filter = filter ?? new ProductSearchFilterDto();
    }
}