namespace CleanSample.Domain.Entities;

public class Role : BaseEntity
{
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

    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}

