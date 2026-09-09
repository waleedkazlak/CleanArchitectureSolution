namespace CleanSample.Application.DTOs;

public class FieldJobSearchFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? JobNumber { get; set; }
    public long? PickRequestId { get; set; }
    public int? ClientId { get; set; }
    public int? ClientLocationId { get; set; }
    public int? TechnicianId { get; set; }
    public int? SupervisorId { get; set; }
    public string? Status { get; set; }
    public bool? Verified { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortDirection { get; set; } = "desc";
}
