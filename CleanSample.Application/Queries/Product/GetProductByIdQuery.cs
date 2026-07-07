using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Product;
public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public int Id { get; set; }
}
