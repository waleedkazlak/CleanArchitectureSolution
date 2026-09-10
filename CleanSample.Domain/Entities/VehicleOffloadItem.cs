namespace CleanSample.Domain.Entities;

public class VehicleOffloadItem : BaseEntity<long>
{
    public long VehicleOffloadId { get; set; }
    public VehicleOffload VehicleOffload { get; set; } = null!;

    public long? LoadId { get; set; }
    public Load? Load { get; set; }

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public string? Barcode { get; set; }

    public decimal Quantity { get; set; }

    public DateTime OffloadedAt { get; set; } = DateTime.UtcNow;
}
