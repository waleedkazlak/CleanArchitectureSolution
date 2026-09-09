namespace CleanSample.Domain.Entities;

public class Screen : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Module { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
