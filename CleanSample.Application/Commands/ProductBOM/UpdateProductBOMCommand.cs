using MediatR;

namespace CleanSample.Application.Commands.ProductBOM;

public class UpdateProductBOMCommand : IRequest<bool>
{
    public int Id { get; set; }
    public int ProductVariantId { get; set; }
    public int PartId { get; set; }
    public decimal Quantity { get; set; }
}
