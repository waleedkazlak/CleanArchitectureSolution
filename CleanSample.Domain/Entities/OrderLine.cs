namespace CleanSample.Domain.Entities;

public class OrderLine : BaseEntity<long>
{
    public long OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductVariantId { get; set; }
    public ProductVariant ProductVariant { get; set; } = null!;

    public int Quantity { get; set; }

    public string? Notes { get; set; }
}
