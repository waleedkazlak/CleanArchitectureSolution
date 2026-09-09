namespace CleanSample.Domain.Entities;

public class VehicleLoadItem : BaseEntity<long>
{
    public long VehicleLoadId { get; set; }
    public VehicleLoad VehicleLoad { get; set; } = null!;

    public long? PickId { get; set; }
    public Pick? Pick { get; set; }

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public string? Barcode { get; set; }

    public decimal Quantity { get; set; }

    public DateTime LoadedAt { get; set; } = DateTime.UtcNow;
}
