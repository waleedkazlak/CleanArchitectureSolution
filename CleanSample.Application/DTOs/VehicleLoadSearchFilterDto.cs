namespace CleanSample.Application.DTOs;

public class VehicleLoadSearchFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public long? PickRequestId { get; set; }
    public int? VehicleId { get; set; }
    public int? DriverId { get; set; }
    public string? Status { get; set; }
    public bool? Verified { get; set; }
    public int? VerifiedBy { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; } = "LoadDate";
    public string? SortDirection { get; set; } = "desc";
}
