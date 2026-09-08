using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.ProductVariant;

public class GetProductVariantByIdQuery : IRequest<ProductVariantDto?>
{
    public int Id { get; set; }
}
