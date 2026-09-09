using CleanSample.Application.Commands.Color;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Color;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Color operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ColorsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ColorsController> _logger;

    public ColorsController(IMediator mediator, ILogger<ColorsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all colors with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Color search filter object</param>
    /// <returns>Paginated list of colors</returns>
    [HttpPost("search")]
    [RequirePermission("COLORS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ColorDto>>>> Search(
        [FromBody] ColorSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching colors", User.Identity?.Name);

        var query = new GetColorsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ColorDto>>()
            .SetSuccess(result, result.TotalCount, "Colors retrieved successfully"));
    }

    /// <summary>
    /// Get all colors with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of colors</returns>
    [HttpGet]
    [RequirePermission("COLORS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ColorDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching colors - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new ColorSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetColorsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ColorDto>>()
            .SetSuccess(result, result.TotalCount, "Colors retrieved successfully"));
    }

    /// <summary>
    /// Get color by id
    /// </summary>
    /// <param name="id">Color id</param>
    /// <returns>Color details</returns>
    [HttpGet("{id}")]
    [RequirePermission("COLORS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<ColorDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching color with id: {ColorId}", User.Identity?.Name, id);

        var query = new GetColorByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<ColorDto>()
                .SetError(404, $"Color with id {id} not found"));
        }

        return Ok(new APIBaseResponse<ColorDto>()
            .SetSuccess(result, "Color retrieved successfully"));
    }

    /// <summary>
    /// Create a new color
    /// </summary>
    /// <param name="command">Color creation command</param>
    /// <returns>Created color id</returns>
    [HttpPost]
    [RequirePermission("COLORS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateColorCommand command)
    {
        _logger.LogInformation("User {User} creating new color: {ColorName}", User.Identity?.Name, command?.Name);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<int>()
                .SetSuccess(result, "Color created successfully"));
    }

    /// <summary>
    /// Update an existing color
    /// </summary>
    /// <param name="id">Color id</param>
    /// <param name="command">Color update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("COLORS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] int id,
        [FromBody] UpdateColorCommand command)
    {
        _logger.LogInformation("User {User} updating color with id: {ColorId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Color with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Color updated successfully"));
    }

    /// <summary>
    /// Delete a color
    /// </summary>
    /// <param name="id">Color id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("COLORS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] int id)
    {
        _logger.LogInformation("User {User} deleting color with id: {ColorId}", User.Identity?.Name, id);

        var command = new DeleteColorCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Color with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Color deleted successfully"));
    }
}
