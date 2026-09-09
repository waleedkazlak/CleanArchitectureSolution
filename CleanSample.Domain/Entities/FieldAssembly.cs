namespace CleanSample.Domain.Entities;

public class FieldAssembly : BaseEntity<long>
{
    public long FieldJobId { get; set; }
    public FieldJob FieldJob { get; set; } = null!;

    public int ProductVariantId { get; set; }
    public ProductVariant ProductVariant { get; set; } = null!;

    public string? ProductBarcode { get; set; }

    public int Quantity { get; set; }

    public DateTime? AssemblyDate { get; set; }

    public string Status { get; set; } = "Pending";

    public int? TechnicianId { get; set; }
    public User? Technician { get; set; }

    public int? SupervisorId { get; set; }
    public User? Supervisor { get; set; }

    public bool Verified { get; set; } = false;

    public DateTime? VerifiedAt { get; set; }

    public string? Notes { get; set; }
}
