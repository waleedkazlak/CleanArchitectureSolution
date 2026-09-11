using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.ProductBOM;

public class GetProductBOMsByProductIdQuery : IRequest<IEnumerable<ProductBOMDto>>
{
    public int ProductId { get; set; }

    public GetProductBOMsByProductIdQuery(int productId)
    {
        ProductId = productId;
    }
}
