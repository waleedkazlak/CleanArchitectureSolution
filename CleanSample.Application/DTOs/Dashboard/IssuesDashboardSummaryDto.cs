namespace CleanSample.Application.DTOs.Dashboard;

public class IssuesDashboardSummaryDto
{
    public int TotalIssues { get; set; }
    public int OpenIssues { get; set; }
    public int InProgressIssues { get; set; }
    public int ResolvedIssues { get; set; }
    public int ClosedIssues { get; set; }
    public int CriticalIssues { get; set; }
    public int HighSeverityIssues { get; set; }
    public int MediumSeverityIssues { get; set; }
    public int LowSeverityIssues { get; set; }
    public Dictionary<string, int> ByStatus { get; set; } = new();
    public Dictionary<string, int> BySeverity { get; set; } = new();
}
