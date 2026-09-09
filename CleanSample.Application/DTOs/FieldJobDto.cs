namespace CleanSample.Application.DTOs;

public class FieldJobDto
{
    public long Id { get; set; }
    public string JobNumber { get; set; } = string.Empty;
    public long PickRequestId { get; set; }
    public string? PickRequestNumber { get; set; }
    public int ClientId { get; set; }
    public string? ClientName { get; set; }
    public int? ClientLocationId { get; set; }
    public string? LocationName { get; set; }
    public int? TechnicianId { get; set; }
    public string? TechnicianName { get; set; }
    public int? SupervisorId { get; set; }
    public string? SupervisorName { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string Status { get; set; } = "Scheduled";
    public bool Verified { get; set; } = false;
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
