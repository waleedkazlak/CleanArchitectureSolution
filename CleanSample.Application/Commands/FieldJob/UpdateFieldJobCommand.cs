using MediatR;

namespace CleanSample.Application.Commands.FieldJob;

public class UpdateFieldJobCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long LoadRequestId { get; set; }
    public int ClientId { get; set; }
    public int? ClientLocationId { get; set; }
    public int? TechnicianId { get; set; }
    public int? SupervisorId { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public int Status { get; set; }
    public bool Verified { get; set; } = false;
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
}
