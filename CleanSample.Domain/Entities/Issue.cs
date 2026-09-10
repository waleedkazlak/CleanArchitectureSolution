namespace CleanSample.Domain.Entities;

public class Issue : BaseEntity<long>
{
    public long? LoadRequestId { get; set; }
    public LoadRequest? LoadRequest { get; set; }

    public long? FieldJobId { get; set; }
    public FieldJob? FieldJob { get; set; }

    public long? FieldAssemblyId { get; set; }
    public FieldAssembly? FieldAssembly { get; set; }

    public string IssueType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Severity { get; set; } = "Medium";

    public string Status { get; set; } = "Open";

    public int? ReportedBy { get; set; }
    public User? ReportedByUser { get; set; }

    public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

    public int? ResolvedBy { get; set; }
    public User? ResolvedByUser { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public string? ResolutionNotes { get; set; }
}
