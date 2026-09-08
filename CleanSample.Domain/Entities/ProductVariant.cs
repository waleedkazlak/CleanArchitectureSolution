namespace CleanSample.Domain.Entities;

public class ProductVariant : BaseEntity
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int? ColorId { get; set; }
    public Color? Color { get; set; }

    public int? MaterialId { get; set; }
    public Material? Material { get; set; }

    public int? DesignId { get; set; }
    public Design? Design { get; set; }

    public string Code { get; set; } = null!;
    public string? Barcode { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
