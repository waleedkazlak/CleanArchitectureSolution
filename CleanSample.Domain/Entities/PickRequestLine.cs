namespace CleanSample.Domain.Entities;

public class PickRequestLine : BaseEntity<long>
{
    public long PickRequestId { get; set; }
    public PickRequest PickRequest { get; set; } = null!;

    public int ProductVariantId { get; set; }
    public ProductVariant ProductVariant { get; set; } = null!;

    public int Quantity { get; set; }

    public ICollection<PickRequestPart> PickRequestParts { get; set; } = new List<PickRequestPart>();
}
