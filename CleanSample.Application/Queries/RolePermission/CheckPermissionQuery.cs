using MediatR;

namespace CleanSample.Application.Queries.RolePermission;

public class CheckPermissionQuery : IRequest<bool>
{
    public int RoleId { get; set; }
    public string ScreenCode { get; set; } = string.Empty;
    public string Action { get; set; } = "view"; // view, create, update, delete

    public CheckPermissionQuery(int roleId, string screenCode, string action)
    {
        RoleId = roleId;
        ScreenCode = screenCode;
        Action = action;
    }
}
