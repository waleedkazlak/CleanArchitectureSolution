using CleanSample.Application.Commands.User;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.User;
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
    /// Create a new user (Admin only)
    /// </summary>
    /// <param name="command">User creation command</param>
    /// <returns>Created user ID</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
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
    /// Update an existing user (Admin only)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="command">User update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
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
    /// Delete a user (Admin only)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
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
    /// Toggle user active/inactive status (Admin only)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>Success status</returns>
    [HttpPatch("{id}/toggle-status")]
    [Authorize(Roles = "Admin")]
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
}
