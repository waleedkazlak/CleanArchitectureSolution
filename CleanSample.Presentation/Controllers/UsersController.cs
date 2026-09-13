using CleanSample.Application.Commands.User;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.User;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for User management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all users with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">User search filter object</param>
    /// <returns>Paginated list of users</returns>
    [HttpPost("search")]
    [RequirePermission("USERS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<UserDto>>>> Search(
        [FromBody] UserSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching users", User.Identity?.Name);

        var query = new GetUsersWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<UserDto>>()
            .SetSuccess(result, result.TotalCount, "Users retrieved successfully"));
    }

    /// <summary>
    /// Get all users with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of users</returns>
    [HttpGet]
    [RequirePermission("USERS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<UserDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching users - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new UserSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetUsersWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<UserDto>>()
            .SetSuccess(result, result.TotalCount, "Users retrieved successfully"));
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User details</returns>
    [HttpGet("{id}")]
    [RequirePermission("USERS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<UserDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching user with id: {UserId}", User.Identity?.Name, id);

        var query = new GetUserByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<UserDto>()
                .SetError(404, $"User with id {id} not found"));
        }

        return Ok(new APIBaseResponse<UserDto>()
            .SetSuccess(result, "User retrieved successfully"));
    }

    /// <summary>
    /// Get users by role ID
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <returns>List of users in the specified role</returns>
    [HttpGet("by-role/{roleId}")]
    [RequirePermission("USERS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<List<UserDto>>>> GetByRoleId([FromRoute] int roleId)
    {
        _logger.LogInformation("User {User} fetching users for RoleId: {RoleId}", User.Identity?.Name, roleId);

        var query = new GetUsersByRoleIdQuery(roleId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<List<UserDto>>()
            .SetSuccess(result, result.Count, "Users retrieved successfully"));
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="command">User creation command</param>
    /// <returns>Created user ID</returns>
    [HttpPost]
    [RequirePermission("USERS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateUserCommand command)
    {
        _logger.LogInformation("User {User} creating new user: {UserName}", User.Identity?.Name, command?.UserName);

        try
        {
            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = result },
                new APIBaseResponse<int>()
                    .SetSuccess(result, "User created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new APIBaseResponse<int>()
                .SetError(400, ex.Message));
        }
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="command">User update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("USERS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] int id,
        [FromBody] UpdateUserCommand command)
    {
        _logger.LogInformation("User {User} updating user with id: {UserId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"User with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "User updated successfully"));
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("USERS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] int id)
    {
        _logger.LogInformation("User {User} deleting user with id: {UserId}", User.Identity?.Name, id);

        var command = new DeleteUserCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"User with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "User deleted successfully"));
    }

    /// <summary>
    /// Toggle user active/inactive status
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>Success status</returns>
    [HttpPatch("{id}/toggle-status")]
    [RequirePermission("USERS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> ToggleStatus([FromRoute] int id)
    {
        _logger.LogInformation("User {User} toggling status for user with id: {UserId}", User.Identity?.Name, id);

        var command = new ToggleUserStatusCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"User with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "User status toggled successfully"));
    }

    /// <summary>
    /// Get current logged in user profile
    /// </summary>
    /// <returns>Current user details</returns>
    [HttpGet("me")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIBaseResponse<UserDto>>> GetCurrentUser()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized(new APIBaseResponse<UserDto>().SetError(401, "User is not authenticated"));
        }

        var filter = new UserSearchFilterDto { SearchTerm = username, PageSize = 5 };
        var query = new GetUsersWithFilterQuery(filter);
        var result = await _mediator.Send(query);
        var user = result.Items.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (user == null)
        {
            return NotFound(new APIBaseResponse<UserDto>().SetError(404, "User profile not found"));
        }

        return Ok(new APIBaseResponse<UserDto>().SetSuccess(user, "User profile retrieved successfully"));
    }

    /// <summary>
    /// Update user preferred language by User ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="language">Preferred language code ('en' or 'ar')</param>
    /// <returns>Success status</returns>
    [HttpPatch("{id}/preferred-language")]
    [HttpPut("{id}/preferred-language")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIBaseResponse<bool>>> UpdatePreferredLanguage(
        [FromRoute] int id,
        [FromBody] UpdatePreferredLanguageDto? dto = null,
        [FromQuery] string? language = null)
    {
        var targetLang = dto?.ResolvedLanguage ?? language;
        if (string.IsNullOrWhiteSpace(targetLang) || targetLang.ToLower() is not ("en" or "ar"))
        {
            return BadRequest(new APIBaseResponse<bool>().SetError(400, "Language must be 'en' or 'ar'"));
        }

        var command = new UpdateUserPreferredLanguageCommand
        {
            Id = id,
            PreferredLanguage = targetLang.Trim().ToLower()
        };

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>().SetError(404, $"User with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>().SetSuccess(true, $"Preferred language updated to {targetLang}"));
    }

    /// <summary>
    /// Update current logged-in user's preferred language
    /// </summary>
    /// <param name="dto">Preferred language payload ('en' or 'ar')</param>
    /// <param name="language">Preferred language code ('en' or 'ar') via query param fallback</param>
    /// <returns>Success status</returns>
    [HttpPut("me/preferred-language")]
    [HttpPatch("me/preferred-language")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIBaseResponse<bool>>> UpdateMyPreferredLanguage(
        [FromBody] UpdatePreferredLanguageDto? dto = null,
        [FromQuery] string? language = null)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized(new APIBaseResponse<bool>().SetError(401, "User is not authenticated"));
        }

        var targetLang = dto?.ResolvedLanguage ?? language;
        if (string.IsNullOrWhiteSpace(targetLang) || targetLang.ToLower() is not ("en" or "ar"))
        {
            return BadRequest(new APIBaseResponse<bool>().SetError(400, "Language must be 'en' or 'ar'"));
        }

        var filter = new UserSearchFilterDto { SearchTerm = username, PageSize = 5 };
        var query = new GetUsersWithFilterQuery(filter);
        var result = await _mediator.Send(query);
        var user = result.Items.FirstOrDefault(u => u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

        if (user == null)
        {
            return NotFound(new APIBaseResponse<bool>().SetError(404, "User profile not found"));
        }

        var command = new UpdateUserPreferredLanguageCommand
        {
            Id = user.Id,
            PreferredLanguage = targetLang.Trim().ToLower()
        };

        var updateResult = await _mediator.Send(command);
        return Ok(new APIBaseResponse<bool>().SetSuccess(updateResult, $"Preferred language updated to {targetLang}"));
    }
}
