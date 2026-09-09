namespace CleanSample.Application.DTOs;

public class ScreenPermissionItemDto
{
    public int ScreenId { get; set; }
    public string ScreenName { get; set; } = string.Empty;
    public string ScreenCode { get; set; } = string.Empty;
    public string? Module { get; set; }
    public string? Description { get; set; }

    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
}

public class RolePermissionMatrixDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public List<ScreenPermissionItemDto> Permissions { get; set; } = new();
}

public class BatchUpdateRolePermissionsDto
{
    public int RoleId { get; set; }
    public List<ScreenPermissionUpdateItemDto> Permissions { get; set; } = new();
}

public class ScreenPermissionUpdateItemDto
{
    public int ScreenId { get; set; }
    public bool CanView { get; set; }
    public bool CanCreate { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
}
