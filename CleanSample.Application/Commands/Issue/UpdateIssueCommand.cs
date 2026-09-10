using MediatR;

namespace CleanSample.Application.Commands.Issue;

public class UpdateIssueCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long? LoadRequestId { get; set; }
    public long? FieldJobId { get; set; }
    public long? FieldAssemblyId { get; set; }
    public string IssueType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = "Medium";
    public string Status { get; set; } = "Open";
    public int? ReportedBy { get; set; }
    public DateTime? ReportedAt { get; set; }
    public int? ResolvedBy { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }
}
