namespace CleanSample.Application.DTOs;

public class CreateProductBOMItemDto
{
    public int? Id { get; set; }
    public int ProductVariantId { get; set; }
    public int PartId { get; set; }
    public decimal Quantity { get; set; }
}
