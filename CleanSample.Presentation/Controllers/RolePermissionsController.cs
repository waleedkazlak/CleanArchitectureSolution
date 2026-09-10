using System.Security.Claims;
using CleanSample.Application.Commands.RolePermission;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.RolePermission;
using CleanSample.Domain.Interfaces;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolePermissionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RolePermissionsController> _logger;

    public RolePermissionsController(
        IMediator mediator,
        IUnitOfWork unitOfWork,
        ILogger<RolePermissionsController> logger)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Get full permission matrix for a specific role
    /// </summary>
    [HttpGet("by-role/{roleId}")]
    [RequirePermission("ROLE_PERMISSIONS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIBaseResponse<RolePermissionMatrixDto>>> GetByRoleId(int roleId)
    {
        var result = await _mediator.Send(new GetRolePermissionMatrixByRoleIdQuery(roleId));
        return Ok(new APIBaseResponse<RolePermissionMatrixDto>().SetSuccess(result, "Role permissions retrieved successfully"));
    }

    /// <summary>
    /// Get permissions matrix for the currently authenticated user
    /// </summary>
    [HttpGet("my-permissions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<APIBaseResponse<RolePermissionMatrixDto>>> GetMyPermissions()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new APIBaseResponse<RolePermissionMatrixDto>().SetError(StatusCodes.Status401Unauthorized, "User not authenticated or invalid token"));
        }

        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null || !user.RoleId.HasValue)
        {
            return Ok(new APIBaseResponse<RolePermissionMatrixDto>().SetSuccess(new RolePermissionMatrixDto
            {
                RoleId = 0,
                RoleName = "None",
                Permissions = new List<ScreenPermissionItemDto>()
            }, "No role assigned"));
        }

        var result = await _mediator.Send(new GetRolePermissionMatrixByRoleIdQuery(user.RoleId.Value));
        return Ok(new APIBaseResponse<RolePermissionMatrixDto>().SetSuccess(result, "Current user permissions retrieved successfully"));
    }

    /// <summary>
    /// Check if a specific role has permission for an action on a screen
    /// </summary>
    [HttpGet("check")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<bool>>> CheckPermission(
        [FromQuery] int roleId,
        [FromQuery] string screenCode,
        [FromQuery] string action = "view")
    {
        var result = await _mediator.Send(new CheckPermissionQuery(roleId, screenCode, action));
        return Ok(new APIBaseResponse<bool>().SetSuccess(result, "Permission evaluated"));
    }

    /// <summary>
    /// Batch update/save role permissions matrix
    /// </summary>
    [HttpPost("batch-update")]
    [RequirePermission("ROLE_PERMISSIONS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIBaseResponse<bool>>> BatchUpdate([FromBody] BatchUpdateRolePermissionsDto request)
    {
        var result = await _mediator.Send(new BatchUpdateRolePermissionsCommand(request));
        return Ok(new APIBaseResponse<bool>().SetSuccess(result, "Role permissions batch updated successfully"));
    }

    /// <summary>
    /// Set or update a single screen permission for a role
    /// </summary>
    [HttpPut]
    [RequirePermission("ROLE_PERMISSIONS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIBaseResponse<RolePermissionDto>>> SetPermission([FromBody] SetRoleScreenPermissionCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new APIBaseResponse<RolePermissionDto>().SetSuccess(result, "Permission updated successfully"));
    }

    /// <summary>
    /// Delete a role permission record
    /// </summary>
    [HttpDelete("{id}")]
    [RequirePermission("ROLE_PERMISSIONS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRolePermissionCommand(id));
        return Ok(new APIBaseResponse<bool>().SetSuccess(result, "Permission deleted successfully"));
    }
}
