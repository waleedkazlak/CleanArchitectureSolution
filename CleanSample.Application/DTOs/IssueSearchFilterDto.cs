namespace CleanSample.Application.DTOs;

public class IssueSearchFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public long? LoadRequestId { get; set; }
    public long? FieldJobId { get; set; }
    public long? FieldAssemblyId { get; set; }
    public string? IssueType { get; set; }
    public string? Severity { get; set; }
    public int? Status { get; set; }
    public int? ReportedBy { get; set; }
    public int? ResolvedBy { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; } = "ReportedAt";
    public string? SortDirection { get; set; } = "desc";
}
