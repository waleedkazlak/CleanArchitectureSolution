namespace CleanSample.Domain.Entities;

public class OrderLine : BaseEntity<long>
{
    public long OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }

    public string? Notes { get; set; }
}
