namespace CleanSample.Application.DTOs;

public class ClientLocationSearchFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public int? ClientId { get; set; }
    public string? Name { get; set; }
    public string? City { get; set; }
    public bool? IsDefault { get; set; }
    public bool? IsActive { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortDirection { get; set; } = "desc";
}
