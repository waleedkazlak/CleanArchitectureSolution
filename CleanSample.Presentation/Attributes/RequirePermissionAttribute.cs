using CleanSample.Application.Common;
using CleanSample.Presentation.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Attributes;

/// <summary>
/// Enforces granular role-based screen permission checks on controller actions
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class RequirePermissionAttribute : TypeFilterAttribute
{
    public RequirePermissionAttribute(string screenCode, PermissionAction action)
        : base(typeof(PermissionAuthorizationFilter))
    {
        Arguments = new object[] { screenCode, action };
        ScreenCode = screenCode;
        Action = action;
    }

    public string ScreenCode { get; }
    public PermissionAction Action { get; }
}
