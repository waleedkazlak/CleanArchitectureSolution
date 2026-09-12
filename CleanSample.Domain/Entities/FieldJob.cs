using CleanSample.Domain.Enums;

namespace CleanSample.Domain.Entities;

public class FieldJob : BaseEntity<long>
{
    public long LoadRequestId { get; set; }
    public LoadRequest LoadRequest { get; set; } = null!;

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int? ClientLocationId { get; set; }
    public ClientLocation? ClientLocation { get; set; }

    public int? TechnicianId { get; set; }
    public User? Technician { get; set; }

    public int? SupervisorId { get; set; }
    public User? Supervisor { get; set; }

    public DateTime? ScheduledDate { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? CompletionDate { get; set; }

    public int Status { get; set; } = (int)FieldJobStatusEnum.Scheduled;
    public FieldJobStatus? FieldJobStatus { get; set; }

    public bool Verified { get; set; } = false;

    public DateTime? VerifiedAt { get; set; }

    public string? Notes { get; set; }

    public ICollection<FieldAssembly> FieldAssemblies { get; set; } = new List<FieldAssembly>();
    public ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
