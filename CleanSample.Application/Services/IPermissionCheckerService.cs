using CleanSample.Application.Common;

namespace CleanSample.Application.Services;

/// <summary>
/// Service for validating whether a user/role has permission for a specific screen and action
/// </summary>
public interface IPermissionCheckerService
{
    /// <summary>
    /// Checks if a role has permission for a specific screen code and action
    /// </summary>
    Task<bool> HasPermissionAsync(
        int? roleId,
        string? roleName,
        string screenCode,
        PermissionAction action,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears any cached permissions for a given role (or all roles if null)
    /// </summary>
    void InvalidateRoleCache(int? roleId = null);
}
