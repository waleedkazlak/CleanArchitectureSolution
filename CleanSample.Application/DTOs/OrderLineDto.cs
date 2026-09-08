namespace CleanSample.Application.DTOs;

public class OrderLineDto
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string? OrderNumber { get; set; }
    public int ProductVariantId { get; set; }
    public string? ProductVariantCode { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
