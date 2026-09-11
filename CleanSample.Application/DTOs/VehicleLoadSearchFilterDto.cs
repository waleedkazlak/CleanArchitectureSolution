namespace CleanSample.Application.DTOs;

public class VehicleLoadSearchFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public long? LoadRequestId { get; set; }
    public int? PartId { get; set; }
    public string? Barcode { get; set; }
    public int? LoadedBy { get; set; }
    public int? DriverId { get; set; }
    public int? VehicleId { get; set; }
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; } = "LoadDate";
    public string? SortDirection { get; set; } = "desc";
}
