namespace CleanSample.Application.DTOs;
public class VehicleOffloadSearchFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public long? LoadRequestId { get; set; }
    public int? PartId { get; set; }
    public int? VehicleId { get; set; }
    public int? DriverId { get; set; }
    public string? Barcode { get; set; }
    public int? Status { get; set; }
    public bool? Verified { get; set; }
    public int? VerifiedBy { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; } = "OffloadDate";
    public string? SortDirection { get; set; } = "desc";
}
