namespace CleanSample.Domain.Entities;

public class VehicleLoad : BaseEntity<long>
{
    public long PickRequestId { get; set; }
    public PickRequest PickRequest { get; set; } = null!;

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public int DriverId { get; set; }
    public User Driver { get; set; } = null!;

    public DateTime LoadDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Loading";

    public bool Verified { get; set; } = false;

    public int? VerifiedBy { get; set; }
    public User? Verifier { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public string? Notes { get; set; }

    public ICollection<VehicleLoadItem> VehicleLoadItems { get; set; } = new List<VehicleLoadItem>();
}
