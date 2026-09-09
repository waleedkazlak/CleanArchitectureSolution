namespace CleanSample.Application.DTOs;

public class FieldAssemblyDto
{
    public long Id { get; set; }
    public long FieldJobId { get; set; }
    public string? JobNumber { get; set; }
    public int ProductVariantId { get; set; }
    public string? ProductVariantCode { get; set; }
    public string? ProductBarcode { get; set; }
    public int Quantity { get; set; }
    public DateTime? AssemblyDate { get; set; }
    public string Status { get; set; } = "Pending";
    public int? TechnicianId { get; set; }
    public string? TechnicianName { get; set; }
    public int? SupervisorId { get; set; }
    public string? SupervisorName { get; set; }
    public bool Verified { get; set; } = false;
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
