namespace CleanSample.Application.DTOs;

public class ProductBOMDto
{
    public int Id { get; set; }
    public int ProductVariantId { get; set; }
    public string? ProductVariantCode { get; set; }
    public int PartId { get; set; }
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
    public decimal Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
