using MediatR;

namespace CleanSample.Application.Commands.ProductBOM;

public class CreateProductBOMCommand : IRequest<int>
{
    public int ProductVariantId { get; set; }
    public int PartId { get; set; }
    public decimal Quantity { get; set; }
}
