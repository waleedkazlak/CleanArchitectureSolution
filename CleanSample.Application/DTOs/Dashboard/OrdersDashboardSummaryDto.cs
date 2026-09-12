namespace CleanSample.Application.DTOs.Dashboard;

public class OrdersDashboardSummaryDto
{
    public int TotalOrders { get; set; }
    public int DraftOrders { get; set; }
    public int PendingOrders { get; set; }
    public int ApprovedOrders { get; set; }
    public int ProcessingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int CancelledOrders { get; set; }
    public Dictionary<string, int> StatusCounts { get; set; } = new();
}
