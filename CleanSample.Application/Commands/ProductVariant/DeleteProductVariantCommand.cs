using MediatR;

namespace CleanSample.Application.Commands.ProductVariant;

public class DeleteProductVariantCommand : IRequest<bool>
{
    public int Id { get; set; }
}
