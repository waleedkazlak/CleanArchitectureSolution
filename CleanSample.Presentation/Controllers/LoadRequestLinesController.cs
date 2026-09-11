using CleanSample.Application.Commands.LoadRequestLine;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.LoadRequestLine;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for LoadRequestLine operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoadRequestLinesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoadRequestLinesController> _logger;

    public LoadRequestLinesController(IMediator mediator, ILogger<LoadRequestLinesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all load request lines with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Load request line search filter object</param>
    /// <returns>Paginated list of load request lines</returns>
    [HttpPost("search")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<LoadRequestLineDto>>>> Search(
        [FromBody] LoadRequestLineSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching load request lines", User.Identity?.Name);

        var query = new GetLoadRequestLinesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<LoadRequestLineDto>>()
            .SetSuccess(result, result.TotalCount, "Load request lines retrieved successfully"));
    }

    /// <summary>
    /// Get all load request lines with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of load request lines</returns>
    [HttpGet]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<LoadRequestLineDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching load request lines - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new LoadRequestLineSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetLoadRequestLinesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<LoadRequestLineDto>>()
            .SetSuccess(result, result.TotalCount, "Load request lines retrieved successfully"));
    }

    /// <summary>
    /// Get load request line by id
    /// </summary>
    /// <param name="id">Load request line id (bigint)</param>
    /// <returns>Load request line details</returns>
    [HttpGet("{id}")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<LoadRequestLineDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching load request line with id: {LoadRequestLineId}", User.Identity?.Name, id);

        var query = new GetLoadRequestLineByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<LoadRequestLineDto>()
                .SetError(404, $"Load request line with id {id} not found"));
        }

        return Ok(new APIBaseResponse<LoadRequestLineDto>()
            .SetSuccess(result, "Load request line retrieved successfully"));
    }

    /// <summary>
    /// Get all load request lines for a specific load request
    /// </summary>
    /// <param name="loadRequestId">Load request id (bigint)</param>
    /// <returns>List of load request lines</returns>
    [HttpGet("by-load-request/{loadRequestId}")]
    [HttpGet("load-request/{loadRequestId}")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<LoadRequestLineDto>>>> GetByLoadRequestId([FromRoute] long loadRequestId)
    {
        _logger.LogInformation("User {User} fetching lines for load request: {LoadRequestId}", User.Identity?.Name, loadRequestId);

        var query = new GetLoadRequestLinesByLoadRequestIdQuery(loadRequestId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<LoadRequestLineDto>>()
            .SetSuccess(result, "Load request lines retrieved successfully"));
    }

    /// <summary>
    /// Create or update load request line records (supports single item or list of items)
    /// </summary>
    /// <param name="command">Load request line creation/update command</param>
    /// <returns>List of created/updated load request lines</returns>
    [HttpPost]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<List<LoadRequestLineDto>>>> Create([FromBody] CreateLoadRequestLineCommand command)
    {
        _logger.LogInformation("User {User} creating/updating load request lines", User.Identity?.Name);

        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<List<LoadRequestLineDto>>()
            .SetSuccess(result, result.Count, "Load request lines processed successfully"));
    }

    /// <summary>
    /// Batch create or update load request lines from a list of items
    /// </summary>
    /// <param name="items">List of load request line items to create or update</param>
    /// <returns>List of created/updated load request lines</returns>
    [HttpPost("batch")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<APIBaseResponse<List<LoadRequestLineDto>>>> Batch([FromBody] List<CreateLoadRequestLineItemDto> items)
    {
        _logger.LogInformation("User {User} batch creating/updating {Count} load request lines", User.Identity?.Name, items?.Count ?? 0);

        var command = new CreateLoadRequestLineCommand(items ?? new List<CreateLoadRequestLineItemDto>());
        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<List<LoadRequestLineDto>>()
            .SetSuccess(result, result.Count, "Load request lines processed successfully"));
    }

    /// <summary>
    /// Update an existing load request line
    /// </summary>
    /// <param name="id">Load request line id (bigint)</param>
    /// <param name="command">Load request line update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] long id,
        [FromBody] UpdateLoadRequestLineCommand command)
    {
        _logger.LogInformation("User {User} updating load request line with id: {LoadRequestLineId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Load request line with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Load request line updated successfully"));
    }

    /// <summary>
    /// Delete a load request line
    /// </summary>
    /// <param name="id">Load request line id (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting load request line with id: {LoadRequestLineId}", User.Identity?.Name, id);

        var command = new DeleteLoadRequestLineCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Load request line with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Load request line deleted successfully"));
    }
}
