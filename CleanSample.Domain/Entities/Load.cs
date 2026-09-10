namespace CleanSample.Domain.Entities;

public class Load : BaseEntity<long>
{
    public long LoadRequestId { get; set; }
    public LoadRequest LoadRequest { get; set; } = null!;

    public long? LoadRequestPartId { get; set; }
    public LoadRequestPart? LoadRequestPart { get; set; }

    public int PartId { get; set; }
    public Part Part { get; set; } = null!;

    public string? Barcode { get; set; }

    public decimal Quantity { get; set; }

    public int? LoadedBy { get; set; }
    public User? Loader { get; set; }

    public int? DriverId { get; set; }
    public User? Driver { get; set; }

    public int? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public DateTime LoadDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Loaded";

    public string? Notes { get; set; }

    public ICollection<VehicleOffloadItem> VehicleOffloadItems { get; set; } = new List<VehicleOffloadItem>();
}
