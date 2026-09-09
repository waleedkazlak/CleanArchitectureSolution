namespace CleanSample.Application.DTOs;

public class IssueDto
{
    public long Id { get; set; }
    public long? PickRequestId { get; set; }
    public string? PickRequestNumber { get; set; }
    public long? FieldJobId { get; set; }
    public string? FieldJobNumber { get; set; }
    public long? FieldAssemblyId { get; set; }
    public string IssueType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = "Medium";
    public string Status { get; set; } = "Open";
    public int? ReportedBy { get; set; }
    public string? ReportedByName { get; set; }
    public DateTime ReportedAt { get; set; }
    public int? ResolvedBy { get; set; }
    public string? ResolvedByName { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
