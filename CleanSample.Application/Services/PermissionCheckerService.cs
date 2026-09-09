using CleanSample.Application.Common;
using CleanSample.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Services;

/// <summary>
/// High-performance cached permission checking service
/// </summary>
public class PermissionCheckerService : IPermissionCheckerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PermissionCheckerService> _logger;

    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
    private const string CacheKeyPrefix = "RolePerms_";

    public PermissionCheckerService(
        IUnitOfWork unitOfWork,
        IMemoryCache cache,
        ILogger<PermissionCheckerService> logger)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> HasPermissionAsync(
        int? roleId,
        string? roleName,
        string screenCode,
        PermissionAction action,
        CancellationToken cancellationToken = default)
    {
        // 1. Admin role always has unrestricted access across all screens
        if (!string.IsNullOrWhiteSpace(roleName) && roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // 2. If no role ID is assigned, deny access
        if (!roleId.HasValue || roleId.Value <= 0)
        {
            _logger.LogWarning("Access denied: User has no valid RoleId assigned.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(screenCode))
        {
            return false;
        }

        // 3. Retrieve or load cached role permission map
        var normalizedScreenCode = screenCode.Trim().ToUpperInvariant();
        var permissionsMap = await GetRolePermissionsMapAsync(roleId.Value, cancellationToken);

        if (permissionsMap == null || !permissionsMap.TryGetValue(normalizedScreenCode, out var perm))
        {
            _logger.LogWarning("Role {RoleId} has no permission entry configured for screen '{ScreenCode}'.", roleId.Value, normalizedScreenCode);
            return false;
        }

        // 4. Evaluate specific action permission
        var hasAccess = action switch
        {
            PermissionAction.View => perm.CanView,
            PermissionAction.Create => perm.CanCreate,
            PermissionAction.Update => perm.CanUpdate,
            PermissionAction.Delete => perm.CanDelete,
            _ => false
        };

        if (!hasAccess)
        {
            _logger.LogWarning("Role {RoleId} lacks {Action} permission for screen '{ScreenCode}'.", roleId.Value, action, normalizedScreenCode);
        }

        return hasAccess;
    }

    public void InvalidateRoleCache(int? roleId = null)
    {
        if (roleId.HasValue)
        {
            _cache.Remove($"{CacheKeyPrefix}{roleId.Value}");
            _logger.LogInformation("Invalidated permission cache for RoleId: {RoleId}", roleId.Value);
        }
    }

    private async Task<Dictionary<string, (bool CanView, bool CanCreate, bool CanUpdate, bool CanDelete)>> GetRolePermissionsMapAsync(
        int roleId,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"{CacheKeyPrefix}{roleId}";

        if (_cache.TryGetValue(cacheKey, out Dictionary<string, (bool CanView, bool CanCreate, bool CanUpdate, bool CanDelete)>? cachedMap) && cachedMap != null)
        {
            return cachedMap;
        }

        var rolePermissions = await _unitOfWork.RolePermissions.GetByRoleIdAsync(roleId, cancellationToken);
        var activeScreens = await _unitOfWork.Screens.GetActiveAsync(cancellationToken);
        var screenDict = activeScreens.ToDictionary(s => s.Id);

        var map = new Dictionary<string, (bool CanView, bool CanCreate, bool CanUpdate, bool CanDelete)>(StringComparer.OrdinalIgnoreCase);

        foreach (var rp in rolePermissions)
        {
            if (screenDict.TryGetValue(rp.ScreenId, out var screen))
            {
                map[screen.Code.ToUpperInvariant()] = (rp.CanView, rp.CanCreate, rp.CanUpdate, rp.CanDelete);
            }
        }

        _cache.Set(cacheKey, map, CacheDuration);
        return map;
    }
}
