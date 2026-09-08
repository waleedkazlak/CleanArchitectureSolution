using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.ProductVariant;

public class GetProductVariantsWithFilterQuery : IRequest<PaginatedResultDto<ProductVariantDto>>
{
    public ProductVariantSearchFilterDto Filter { get; set; } = new();

    public GetProductVariantsWithFilterQuery()
    {
    }

    public GetProductVariantsWithFilterQuery(ProductVariantSearchFilterDto filter)
    {
        Filter = filter ?? new ProductVariantSearchFilterDto();
    }
}
