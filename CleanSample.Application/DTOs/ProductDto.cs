namespace CleanSample.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int? ColorId { get; set; }
    public string? ColorName { get; set; }
    public int? MaterialId { get; set; }
    public string? MaterialName { get; set; }
    public int? DesignId { get; set; }
    public string? DesignName { get; set; }
    public string Name { get; set; } = null!;
    public string? Barcode { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
