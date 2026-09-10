namespace CleanSample.Domain.Entities;

public class LoadRequestPart : BaseEntity<long>
{
    public long LoadRequestId { get; set; }
    public LoadRequest LoadRequest { get; set; } = null!;

    public long LoadRequestLineId { get; set; }
    public LoadRequestLine LoadRequestLine { get; set; } = null!;

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public decimal RequiredQuantity { get; set; }
    public decimal LoadedQuantity { get; set; } = 0;

    public string Status { get; set; } = "Pending";
    public ICollection<Load> Loads { get; set; } = new List<Load>();
}
