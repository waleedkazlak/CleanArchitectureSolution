namespace CleanSample.Domain.Entities;

public class PickRequestPart : BaseEntity<long>
{
    public long PickRequestId { get; set; }
    public PickRequest PickRequest { get; set; } = null!;

    public long PickRequestLineId { get; set; }
    public PickRequestLine PickRequestLine { get; set; } = null!;

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public decimal RequiredQuantity { get; set; }
    public decimal PickedQuantity { get; set; } = 0;

    public string Status { get; set; } = "Pending";
    public ICollection<Pick> Picks { get; set; } = new List<Pick>();
}
