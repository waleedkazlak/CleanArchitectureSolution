namespace CleanSample.Application.DTOs.Dashboard;

public class LoadRequestsDashboardSummaryDto
{
    public int TotalLoadRequests { get; set; }
    public int NewLoadRequests { get; set; }
    public int LoadedLoadRequests { get; set; }
    public int LoadingLoadRequests { get; set; }
    public int OffloadedLoadRequests { get; set; }
    public int CompletedLoadRequests { get; set; }
    public int BlockedLoadRequests { get; set; }
    public int CancelledLoadRequests { get; set; }
    public int VerifiedLoadRequests { get; set; }
    public int UnverifiedLoadRequests { get; set; }
    public Dictionary<string, int> StatusCounts { get; set; } = new();
}
