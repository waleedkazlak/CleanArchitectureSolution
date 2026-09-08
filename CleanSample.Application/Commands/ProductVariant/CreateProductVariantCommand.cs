using MediatR;

namespace CleanSample.Application.Commands.ProductVariant;

public class CreateProductVariantCommand : IRequest<int>
{
    public int ProductId { get; set; }
    public int? ColorId { get; set; }
    public int? MaterialId { get; set; }
    public int? DesignId { get; set; }
    public string Code { get; set; } = null!;
    public string? Barcode { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
