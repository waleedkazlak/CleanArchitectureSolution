namespace CleanSample.Domain.Entities;

public class ProductBOM : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public decimal Quantity { get; set; }
}
