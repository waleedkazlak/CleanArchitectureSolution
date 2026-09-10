namespace CleanSample.Domain.Entities;

public class VehicleOffload : BaseEntity<long>
{
    public long LoadRequestId { get; set; }
    public LoadRequest LoadRequest { get; set; } = null!;

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public int DriverId { get; set; }
    public User Driver { get; set; } = null!;

    public DateTime OffloadDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Offloading";

    public bool Verified { get; set; } = false;

    public int? VerifiedBy { get; set; }
    public User? Verifier { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public string? Notes { get; set; }

    public ICollection<VehicleOffloadItem> VehicleOffloadItems { get; set; } = new List<VehicleOffloadItem>();
}
