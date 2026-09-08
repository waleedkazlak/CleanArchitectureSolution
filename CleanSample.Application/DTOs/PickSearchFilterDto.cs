namespace CleanSample.Application.DTOs;

public class PickSearchFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public long? PickRequestId { get; set; }
    public long? PickRequestPartId { get; set; }
    public int? PartId { get; set; }
    public string? Barcode { get; set; }
    public int? PickedBy { get; set; }
    public int? DriverId { get; set; }
    public int? VehicleId { get; set; }
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; } = "PickDate";
    public string? SortDirection { get; set; } = "desc";
}
