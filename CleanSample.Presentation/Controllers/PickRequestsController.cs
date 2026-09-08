using CleanSample.Application.Commands.PickRequest;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.PickRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for PickRequest operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PickRequestsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PickRequestsController> _logger;

    public PickRequestsController(IMediator mediator, ILogger<PickRequestsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all pick requests with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Pick request search filter object</param>
    /// <returns>Paginated list of pick requests</returns>
    [HttpPost("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<PickRequestDto>>>> Search(
        [FromBody] PickRequestSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching pick requests", User.Identity?.Name);

        var query = new GetPickRequestsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<PickRequestDto>>()
            .SetSuccess(result, result.TotalCount, "Pick requests retrieved successfully"));
    }

    /// <summary>
    /// Get all pick requests with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of pick requests</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<PickRequestDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching pick requests - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new PickRequestSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetPickRequestsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<PickRequestDto>>()
            .SetSuccess(result, result.TotalCount, "Pick requests retrieved successfully"));
    }

    /// <summary>
    /// Get pick request by id
    /// </summary>
    /// <param name="id">Pick request id (bigint)</param>
    /// <returns>Pick request details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PickRequestDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching pick request with id: {PickRequestId}", User.Identity?.Name, id);

        var query = new GetPickRequestByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<PickRequestDto>()
                .SetError(404, $"Pick request with id {id} not found"));
        }

        return Ok(new APIBaseResponse<PickRequestDto>()
            .SetSuccess(result, "Pick request retrieved successfully"));
    }

    /// <summary>
    /// Get all pick requests for a specific order
    /// </summary>
    /// <param name="orderId">Order id (bigint)</param>
    /// <returns>List of pick requests</returns>
    [HttpGet("order/{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<PickRequestDto>>>> GetByOrderId([FromRoute] long orderId)
    {
        _logger.LogInformation("User {User} fetching pick requests for order: {OrderId}", User.Identity?.Name, orderId);

        var query = new GetPickRequestsByOrderIdQuery(orderId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<PickRequestDto>>()
            .SetSuccess(result, "Pick requests for order retrieved successfully"));
    }

    /// <summary>
    /// Get all pick requests for a specific client
    /// </summary>
    /// <param name="clientId">Client id</param>
    /// <returns>List of pick requests</returns>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<PickRequestDto>>>> GetByClientId([FromRoute] int clientId)
    {
        _logger.LogInformation("User {User} fetching pick requests for client: {ClientId}", User.Identity?.Name, clientId);

        var query = new GetPickRequestsByClientIdQuery(clientId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<PickRequestDto>>()
            .SetSuccess(result, "Pick requests for client retrieved successfully"));
    }

    /// <summary>
    /// Create a new pick request (Admin only)
    /// </summary>
    /// <param name="command">Pick request creation command</param>
    /// <returns>Created pick request id (bigint)</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<long>>> Create([FromBody] CreatePickRequestCommand command)
    {
        _logger.LogInformation("User {User} creating new pick request: {RequestNumber}", User.Identity?.Name, command?.RequestNumber);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<long>()
                .SetSuccess(result, "Pick request created successfully"));
    }

    /// <summary>
    /// Update an existing pick request (Admin only)
    /// </summary>
    /// <param name="id">Pick request id (bigint)</param>
    /// <param name="command">Pick request update command</param>
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
        [FromRoute] long id,
        [FromBody] UpdatePickRequestCommand command)
    {
        _logger.LogInformation("User {User} updating pick request with id: {PickRequestId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Pick request with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Pick request updated successfully"));
    }

    /// <summary>
    /// Delete a pick request (Admin only)
    /// </summary>
    /// <param name="id">Pick request id (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting pick request with id: {PickRequestId}", User.Identity?.Name, id);

        var command = new DeletePickRequestCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Pick request with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Pick request deleted successfully"));
    }
}
