using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.ProductBOM;

public class GetProductBOMsWithFilterQuery : IRequest<PaginatedResultDto<ProductBOMDto>>
{
    public ProductBOMSearchFilterDto Filter { get; set; }

    public GetProductBOMsWithFilterQuery(ProductBOMSearchFilterDto filter)
    {
        Filter = filter;
    }
}
