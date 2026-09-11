namespace CleanSample.Application.DTOs;

/// <summary>
/// DTO for product search and filtering
/// </summary>
public class ProductSearchFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? Name { get; set; }
    public string? Barcode { get; set; }
    public int? CategoryId { get; set; }
    public int? ColorId { get; set; }
    public int? MaterialId { get; set; }
    public int? DesignId { get; set; }
    public bool? IsActive { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortDirection { get; set; } = "desc";
}