namespace CleanSample.Application.DTOs.Dashboard;

public class VehicleOperationsDashboardSummaryDto
{
    public int TotalLoads { get; set; }
    public decimal TotalLoadedQuantity { get; set; }
    public int TotalOffloads { get; set; }
    public int TotalOffloadedPartsCount { get; set; }
    public int VerifiedOffloads { get; set; }
    public int UnverifiedOffloads { get; set; }
    public Dictionary<string, int> LoadsByStatus { get; set; } = new();
    public Dictionary<string, int> OffloadsByStatus { get; set; } = new();
}
