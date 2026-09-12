namespace CleanSample.Application.DTOs;
public class CreateFieldAssemblyItemDto
{
    public long? Id { get; set; }
    public long FieldJobId { get; set; }
    public int ProductId { get; set; }
    public string? ProductBarcode { get; set; }
    public int Quantity { get; set; }
    public DateTime? AssemblyDate { get; set; }
    public int? Status { get; set; }
    public int? TechnicianId { get; set; }
    public int? SupervisorId { get; set; }
    public bool Verified { get; set; } = false;
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
}
