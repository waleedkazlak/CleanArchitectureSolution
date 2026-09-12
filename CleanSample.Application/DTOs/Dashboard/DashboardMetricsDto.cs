namespace CleanSample.Application.DTOs.Dashboard;

public class DashboardMetricsDto
{
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public OrdersDashboardSummaryDto Orders { get; set; } = new();
    public LoadRequestsDashboardSummaryDto LoadRequests { get; set; } = new();
    public IssuesDashboardSummaryDto Issues { get; set; } = new();
    public VehicleOperationsDashboardSummaryDto VehicleOperations { get; set; } = new();
    public FieldOperationsDashboardSummaryDto FieldOperations { get; set; } = new();
}
