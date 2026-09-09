namespace CleanSample.Domain.Entities;

public class Pick : BaseEntity<long>
{
    public long PickRequestId { get; set; }
    public PickRequest PickRequest { get; set; } = null!;

    public long? PickRequestPartId { get; set; }
    public PickRequestPart? PickRequestPart { get; set; }

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public string? Barcode { get; set; }

    public decimal Quantity { get; set; }

    public int? PickedBy { get; set; }
    public User? Picker { get; set; }

    public int? DriverId { get; set; }
    public User? Driver { get; set; }

    public int? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public DateTime PickDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Picked";

    public string? Notes { get; set; }

    public ICollection<VehicleLoadItem> VehicleLoadItems { get; set; } = new List<VehicleLoadItem>();
}
