namespace CleanSample.Application.DTOs;

public class PickRequestSearchFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? RequestNumber { get; set; }
    public long? OrderId { get; set; }
    public int? ClientId { get; set; }
    public int? ClientLocationId { get; set; }
    public int? RequestedBy { get; set; }
    public int? DriverId { get; set; }
    public int? VehicleId { get; set; }
    public string? Status { get; set; }
    public bool? Verified { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortDirection { get; set; } = "desc";
}
