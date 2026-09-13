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

    public string NameEn { get; set; } = null!;
    public string? NameAr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }

    public string Name
    {
        get => NameEn;
        set => NameEn = value;
    }
    public string? Description
    {
        get => DescriptionEn;
        set => DescriptionEn = value;
    }

    public string? Barcode { get; set; }
    public string? PictureUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<FieldAssembly> FieldAssemblies { get; set; } = new List<FieldAssembly>();
}

