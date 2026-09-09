using CleanSample.Application.Commands.Design;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Design;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Design operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DesignsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DesignsController> _logger;

    public DesignsController(IMediator mediator, ILogger<DesignsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all designs with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Design search filter object</param>
    /// <returns>Paginated list of designs</returns>
    [HttpPost("search")]
    [RequirePermission("DESIGNS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<DesignDto>>>> Search(
        [FromBody] DesignSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching designs", User.Identity?.Name);

        var query = new GetDesignsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<DesignDto>>()
            .SetSuccess(result, result.TotalCount, "Designs retrieved successfully"));
    }

    /// <summary>
    /// Get all designs with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of designs</returns>
    [HttpGet]
    [RequirePermission("DESIGNS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<DesignDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching designs - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new DesignSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetDesignsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<DesignDto>>()
            .SetSuccess(result, result.TotalCount, "Designs retrieved successfully"));
    }

    /// <summary>
    /// Get design by id
    /// </summary>
    /// <param name="id">Design id</param>
    /// <returns>Design details</returns>
    [HttpGet("{id}")]
    [RequirePermission("DESIGNS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<DesignDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching design with id: {DesignId}", User.Identity?.Name, id);

        var query = new GetDesignByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<DesignDto>()
                .SetError(404, $"Design with id {id} not found"));
        }

        return Ok(new APIBaseResponse<DesignDto>()
            .SetSuccess(result, "Design retrieved successfully"));
    }

    /// <summary>
    /// Create a new design
    /// </summary>
    /// <param name="command">Design creation command</param>
    /// <returns>Created design id</returns>
    [HttpPost]
    [RequirePermission("DESIGNS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateDesignCommand command)
    {
        _logger.LogInformation("User {User} creating new design: {DesignName}", User.Identity?.Name, command?.Name);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<int>()
                .SetSuccess(result, "Design created successfully"));
    }

    /// <summary>
    /// Update an existing design
    /// </summary>
    /// <param name="id">Design id</param>
    /// <param name="command">Design update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("DESIGNS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] int id,
        [FromBody] UpdateDesignCommand command)
    {
        _logger.LogInformation("User {User} updating design with id: {DesignId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Design with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Design updated successfully"));
    }

    /// <summary>
    /// Delete a design
    /// </summary>
    /// <param name="id">Design id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("DESIGNS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] int id)
    {
        _logger.LogInformation("User {User} deleting design with id: {DesignId}", User.Identity?.Name, id);

        var command = new DeleteDesignCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Design with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Design deleted successfully"));
    }
}
