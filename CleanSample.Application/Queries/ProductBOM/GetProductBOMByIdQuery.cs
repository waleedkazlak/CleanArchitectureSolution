using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.ProductBOM;

public class GetProductBOMByIdQuery : IRequest<ProductBOMDto?>
{
    public int Id { get; set; }

    public GetProductBOMByIdQuery(int id)
    {
        Id = id;
    }
}
