namespace CleanSample.Application.DTOs;

public class FieldAssemblySearchFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public long? FieldJobId { get; set; }
    public int? ProductVariantId { get; set; }
    public string? ProductBarcode { get; set; }
    public int? TechnicianId { get; set; }
    public int? SupervisorId { get; set; }
    public string? Status { get; set; }
    public bool? Verified { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortDirection { get; set; } = "desc";
}
