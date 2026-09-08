using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.ProductBOM;

public class GetProductBOMsByVariantIdQuery : IRequest<IEnumerable<ProductBOMDto>>
{
    public int ProductVariantId { get; set; }

    public GetProductBOMsByVariantIdQuery(int productVariantId)
    {
        ProductVariantId = productVariantId;
    }
}
