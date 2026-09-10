namespace CleanSample.Domain.Entities;

public class LoadRequestLine : BaseEntity<long>
{
    public long LoadRequestId { get; set; }
    public LoadRequest LoadRequest { get; set; } = null!;

    public int ProductVariantId { get; set; }
    public ProductVariant ProductVariant { get; set; } = null!;

    public int Quantity { get; set; }

    public ICollection<LoadRequestPart> LoadRequestParts { get; set; } = new List<LoadRequestPart>();
}
