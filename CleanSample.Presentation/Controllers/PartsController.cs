using CleanSample.Application.Commands.Part;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Part;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Part operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PartsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PartsController> _logger;

    public PartsController(IMediator mediator, ILogger<PartsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all parts with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Part search filter object</param>
    /// <returns>Paginated list of parts</returns>
    [HttpPost("search")]
    [RequirePermission("PARTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<PartDto>>>> Search(
        [FromBody] PartSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching parts", User.Identity?.Name);

        var query = new GetPartsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<PartDto>>()
            .SetSuccess(result, result.TotalCount, "Parts retrieved successfully"));
    }

    /// <summary>
    /// Get all parts with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of parts</returns>
    [HttpGet]
    [RequirePermission("PARTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<PartDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching parts - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new PartSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetPartsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<PartDto>>()
            .SetSuccess(result, result.TotalCount, "Parts retrieved successfully"));
    }

    /// <summary>
    /// Get part by id
    /// </summary>
    /// <param name="id">Part id</param>
    /// <returns>Part details</returns>
    [HttpGet("{id}")]
    [RequirePermission("PARTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PartDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching part with id: {PartId}", User.Identity?.Name, id);

        var query = new GetPartByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<PartDto>()
                .SetError(404, $"Part with id {id} not found"));
        }

        return Ok(new APIBaseResponse<PartDto>()
            .SetSuccess(result, "Part retrieved successfully"));
    }

    /// <summary>
    /// Create a new part
    /// </summary>
    /// <param name="command">Part creation command</param>
    /// <returns>Created part id</returns>
    [HttpPost]
    [RequirePermission("PARTS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreatePartCommand command)
    {
        _logger.LogInformation("User {User} creating new part: {PartCode}", User.Identity?.Name, command?.Code);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<int>()
                .SetSuccess(result, "Part created successfully"));
    }

    /// <summary>
    /// Update an existing part
    /// </summary>
    /// <param name="id">Part id</param>
    /// <param name="command">Part update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("PARTS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] int id,
        [FromBody] UpdatePartCommand command)
    {
        _logger.LogInformation("User {User} updating part with id: {PartId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Part with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Part updated successfully"));
    }

    /// <summary>
    /// Delete a part
    /// </summary>
    /// <param name="id">Part id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("PARTS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] int id)
    {
        _logger.LogInformation("User {User} deleting part with id: {PartId}", User.Identity?.Name, id);

        var command = new DeletePartCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Part with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Part deleted successfully"));
    }
}
