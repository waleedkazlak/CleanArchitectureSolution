namespace CleanSample.Application.DTOs.Dashboard;

public class FieldOperationsDashboardSummaryDto
{
    public int TotalJobs { get; set; }
    public int ScheduledJobs { get; set; }
    public int InProgressJobs { get; set; }
    public int CompletedJobs { get; set; }
    public int VerifiedJobs { get; set; }
    public int UnverifiedJobs { get; set; }
    public int TotalAssemblies { get; set; }
    public int TotalAssembledQuantity { get; set; }
    public int PendingAssemblies { get; set; }
    public int CompletedAssemblies { get; set; }
    public int VerifiedAssemblies { get; set; }
    public int UnverifiedAssemblies { get; set; }
    public Dictionary<string, int> JobsByStatus { get; set; } = new();
    public Dictionary<string, int> AssembliesByStatus { get; set; } = new();
}
