namespace CleanSample.Application.DTOs;

public class RolePermissionDto
{
    public int RolePermissionId { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;

    public int ScreenId { get; set; }
    public string ScreenName { get; set; } = string.Empty;
    public string ScreenCode { get; set; } = string.Empty;
    public string? Module { get; set; }

    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
