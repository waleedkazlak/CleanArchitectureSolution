using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Product;

public class GetProductsByCategoryIdQuery : IRequest<IEnumerable<ProductDto>>
{
    public int CategoryId { get; set; }

    public GetProductsByCategoryIdQuery(int categoryId)
    {
        CategoryId = categoryId;
    }
}
