namespace CleanSample.Domain.Entities;

public class FieldJob : BaseEntity<long>
{
    public string JobNumber { get; set; } = null!;

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

    public string Status { get; set; } = "Scheduled";

    public bool Verified { get; set; } = false;

    public DateTime? VerifiedAt { get; set; }

    public string? Notes { get; set; }

    public ICollection<FieldAssembly> FieldAssemblies { get; set; } = new List<FieldAssembly>();
    public ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
