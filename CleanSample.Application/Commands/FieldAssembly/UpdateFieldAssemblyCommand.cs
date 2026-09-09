using MediatR;

namespace CleanSample.Application.Commands.FieldAssembly;

public class UpdateFieldAssemblyCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long FieldJobId { get; set; }
    public int ProductVariantId { get; set; }
    public string? ProductBarcode { get; set; }
    public int Quantity { get; set; }
    public DateTime? AssemblyDate { get; set; }
    public string Status { get; set; } = "Pending";
    public int? TechnicianId { get; set; }
    public int? SupervisorId { get; set; }
    public bool Verified { get; set; } = false;
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
}
