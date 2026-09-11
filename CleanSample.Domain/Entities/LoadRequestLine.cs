namespace CleanSample.Domain.Entities;

public class LoadRequestLine : BaseEntity<long>
{
    public long LoadRequestId { get; set; }
    public LoadRequest LoadRequest { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }

    public ICollection<LoadRequestPart> LoadRequestParts { get; set; } = new List<LoadRequestPart>();
}
