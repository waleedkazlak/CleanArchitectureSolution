namespace CleanSample.Domain.Entities;

public class Product : BaseEntity
{
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int? ColorId { get; set; }
    public Color? Color { get; set; }

    public int? MaterialId { get; set; }
    public Material? Material { get; set; }

    public int? DesignId { get; set; }
    public Design? Design { get; set; }

    public string Name { get; set; } = null!;
    public string? Barcode { get; set; }
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<FieldAssembly> FieldAssemblies { get; set; } = new List<FieldAssembly>();
}

