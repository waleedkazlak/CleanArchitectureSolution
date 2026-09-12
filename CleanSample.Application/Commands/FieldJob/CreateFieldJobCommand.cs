using MediatR;

namespace CleanSample.Application.Commands.FieldJob;

public class CreateFieldJobCommand : IRequest<long>
{
    public long LoadRequestId { get; set; }
    public int ClientId { get; set; }
    public int? ClientLocationId { get; set; }
    public int? TechnicianId { get; set; }
    public int? SupervisorId { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public int Status { get; set; } = (int)Domain.Enums.FieldJobStatusEnum.Scheduled;
    public bool Verified { get; set; } = false;
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
}
