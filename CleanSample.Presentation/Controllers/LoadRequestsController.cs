using CleanSample.Application.Commands.LoadRequest;
using CleanSample.Application.Commands.LoadRequestLine;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.LoadRequest;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for LoadRequest operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoadRequestsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoadRequestsController> _logger;

    public LoadRequestsController(IMediator mediator, ILogger<LoadRequestsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all load requests with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Load request search filter object</param>
    /// <returns>Paginated list of load requests</returns>
    [HttpPost("search")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<LoadRequestDto>>>> Search(
        [FromBody] LoadRequestSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching load requests", User.Identity?.Name);

        var query = new GetLoadRequestsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<LoadRequestDto>>()
            .SetSuccess(result, result.TotalCount, "Load requests retrieved successfully"));
    }

    /// <summary>
    /// Get all load requests with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of load requests</returns>
    [HttpGet]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<LoadRequestDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching load requests - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new LoadRequestSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetLoadRequestsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<LoadRequestDto>>()
            .SetSuccess(result, result.TotalCount, "Load requests retrieved successfully"));
    }

    /// <summary>
    /// Get load request by id
    /// </summary>
    /// <param name="id">Load request id (bigint)</param>
    /// <returns>Load request details</returns>
    [HttpGet("{id}")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<LoadRequestDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching load request with id: {LoadRequestId}", User.Identity?.Name, id);

        var query = new GetLoadRequestByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<LoadRequestDto>()
                .SetError(404, $"Load request with id {id} not found"));
        }

        return Ok(new APIBaseResponse<LoadRequestDto>()
            .SetSuccess(result, "Load request retrieved successfully"));
    }

    /// <summary>
    /// Get all load requests for a specific order
    /// </summary>
    /// <param name="orderId">Order id (bigint)</param>
    /// <returns>List of load requests</returns>
    [HttpGet("order/{orderId}")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<LoadRequestDto>>>> GetByOrderId([FromRoute] long orderId)
    {
        _logger.LogInformation("User {User} fetching load requests for order: {OrderId}", User.Identity?.Name, orderId);

        var query = new GetLoadRequestsByOrderIdQuery(orderId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<LoadRequestDto>>()
            .SetSuccess(result, "Load requests for order retrieved successfully"));
    }

    /// <summary>
    /// Get all load requests for a specific client
    /// </summary>
    /// <param name="clientId">Client id</param>
    /// <returns>List of load requests</returns>
    [HttpGet("client/{clientId}")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<LoadRequestDto>>>> GetByClientId([FromRoute] int clientId)
    {
        _logger.LogInformation("User {User} fetching load requests for client: {ClientId}", User.Identity?.Name, clientId);

        var query = new GetLoadRequestsByClientIdQuery(clientId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<LoadRequestDto>>()
            .SetSuccess(result, "Load requests for client retrieved successfully"));
    }

    /// <summary>
    /// Create a new load request
    /// </summary>
    /// <param name="command">Load request creation command</param>
    /// <returns>Created load request id (bigint)</returns>
    [HttpPost]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<long>>> Create([FromBody] CreateLoadRequestCommand command)
    {
        _logger.LogInformation("User {User} creating new load request", User.Identity?.Name);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<long>()
                .SetSuccess(result, "Load request created successfully"));
    }

    /// <summary>
    /// Update an existing load request
    /// </summary>
    /// <param name="id">Load request id (bigint)</param>
    /// <param name="command">Load request update command</param>
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
        [FromBody] UpdateLoadRequestCommand command)
    {
        _logger.LogInformation("User {User} updating load request with id: {LoadRequestId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Load request with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Load request updated successfully"));
    }

    /// <summary>
    /// Delete a load request
    /// </summary>
    /// <param name="id">Load request id (bigint)</param>
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
        _logger.LogInformation("User {User} deleting load request with id: {LoadRequestId}", User.Identity?.Name, id);

        var command = new DeleteLoadRequestCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Load request with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Load request deleted successfully"));
    }

    /// <summary>
    /// Batch create or update load request lines for a specific load request
    /// </summary>
    /// <param name="id">Load request id (bigint)</param>
    /// <param name="items">List of load request line items to create or update</param>
    /// <returns>List of created/updated load request lines</returns>
    [HttpPost("{id}/lines/batch")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<APIBaseResponse<List<LoadRequestLineDto>>>> BatchLoadRequestLinesForLoadRequest(
        [FromRoute] long id,
        [FromBody] List<CreateLoadRequestLineItemDto> items)
    {
        _logger.LogInformation("User {User} batch creating/updating {Count} lines for load request {LoadRequestId}", User.Identity?.Name, items?.Count ?? 0, id);

        if (items != null)
        {
            foreach (var item in items)
            {
                if (item.LoadRequestId <= 0)
                {
                    item.LoadRequestId = id;
                }
            }
        }

        var command = new CreateLoadRequestLineCommand(items ?? new List<CreateLoadRequestLineItemDto>());
        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<List<LoadRequestLineDto>>()
            .SetSuccess(result, result.Count, "Load request lines processed successfully"));
    }

    /// <summary>
    /// Batch create or update load request lines from a list of items
    /// </summary>
    /// <param name="items">List of load request line items to create or update</param>
    /// <returns>List of created/updated load request lines</returns>
    [HttpPost("lines/batch")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<APIBaseResponse<List<LoadRequestLineDto>>>> BatchLoadRequestLines(
        [FromBody] List<CreateLoadRequestLineItemDto> items)
    {
        _logger.LogInformation("User {User} batch creating/updating {Count} load request lines", User.Identity?.Name, items?.Count ?? 0);

        var command = new CreateLoadRequestLineCommand(items ?? new List<CreateLoadRequestLineItemDto>());
        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<List<LoadRequestLineDto>>()
            .SetSuccess(result, result.Count, "Load request lines processed successfully"));
    }
}
