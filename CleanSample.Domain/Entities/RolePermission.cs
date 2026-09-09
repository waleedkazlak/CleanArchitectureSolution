namespace CleanSample.Domain.Entities;

public class RolePermission : BaseEntity
{
    public int RoleId { get; set; }
    public int ScreenId { get; set; }

    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }

    // Navigation properties
    public Role Role { get; set; } = null!;
    public Screen Screen { get; set; } = null!;
}
