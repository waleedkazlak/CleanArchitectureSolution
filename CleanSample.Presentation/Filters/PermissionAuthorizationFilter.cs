using System.Security.Claims;
using CleanSample.Application.Common;
using CleanSample.Application.Services;
using CleanSample.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace CleanSample.Presentation.Filters;

/// <summary>
/// Action filter that validates whether the current authenticated user has permission for a specific screen and action
/// </summary>
public class PermissionAuthorizationFilter : IAsyncActionFilter
{
    private readonly string _screenCode;
    private readonly PermissionAction _action;
    private readonly IPermissionCheckerService _permissionCheckerService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PermissionAuthorizationFilter> _logger;

    public PermissionAuthorizationFilter(
        string screenCode,
        PermissionAction action,
        IPermissionCheckerService permissionCheckerService,
        IUnitOfWork unitOfWork,
        ILogger<PermissionAuthorizationFilter> logger)
    {
        _screenCode = screenCode;
        _action = action;
        _permissionCheckerService = permissionCheckerService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;

        // 1. Check if user is authenticated
        if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
        {
            _logger.LogWarning("Unauthorized access attempt to {ScreenCode}.{Action}: User is not authenticated.", _screenCode, _action);

            context.Result = new ObjectResult(new
            {
                success = false,
                message = "Authentication required. Please provide a valid authorization token.",
                statusCode = 401
            })
            {
                StatusCode = 401
            };
            return;
        }

        // 2. Extract Role Name and Role ID from claims
        var roleName = user.FindFirstValue(ClaimTypes.Role) ?? user.FindFirstValue("role");
        int? roleId = null;

        var roleIdClaim = user.FindFirstValue("RoleId") ?? user.FindFirstValue("roleId");
        if (!string.IsNullOrEmpty(roleIdClaim) && int.TryParse(roleIdClaim, out var parsedRoleId))
        {
            roleId = parsedRoleId;
        }

        // 3. Admin role automatically bypasses all checks
        if (!string.IsNullOrWhiteSpace(roleName) && roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            await next();
            return;
        }

        // 4. Fallback: if RoleId was not in token claims, query user from database once
        if (!roleId.HasValue)
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out var userId))
            {
                var dbUser = await _unitOfWork.Users.GetByIdAsync(userId);
                if (dbUser != null)
                {
                    roleId = dbUser.RoleId;
                    if (string.IsNullOrEmpty(roleName) && dbUser.Role != null)
                    {
                        roleName = dbUser.Role.Name;
                    }
                }
            }
        }

        // 5. Evaluate permissions using the cached permission checker service
        var hasPermission = await _permissionCheckerService.HasPermissionAsync(
            roleId,
            roleName,
            _screenCode,
            _action,
            context.HttpContext.RequestAborted);

        if (!hasPermission)
        {
            _logger.LogWarning(
                "Access denied for user '{Username}' (Role: '{Role}', RoleId: {RoleId}) on {ScreenCode}.{Action}.",
                user.Identity.Name, roleName ?? "None", roleId?.ToString() ?? "None", _screenCode, _action);

            context.Result = new ObjectResult(new
            {
                success = false,
                message = $"Access denied: You do not have permission to '{_action.ToString().ToUpperInvariant()}' the '{_screenCode}' resource.",
                screenCode = _screenCode,
                action = _action.ToString().ToUpperInvariant(),
                requiredPermission = $"{_screenCode}.{_action.ToString().ToUpperInvariant()}",
                statusCode = 403
            })
            {
                StatusCode = 403
            };
            return;
        }

        // Permission check passed
        await next();
    }
}
