using CleanSample.Application.Commands.Load;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Load;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Mobile App Load operations (supports batch/list CRUD operations)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoadsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoadsController> _logger;

    public LoadsController(IMediator mediator, ILogger<LoadsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Search and filter loads with pagination
    /// </summary>
    /// <param name="filter">Load search filter parameters</param>
    /// <returns>Paginated list of loads</returns>
    [HttpPost("search")]
    [RequirePermission("LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<LoadDto>>>> Search(
        [FromBody] LoadSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching loads", User.Identity?.Name);

        var query = new GetLoadsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<LoadDto>>()
            .SetSuccess(result, result.TotalCount, "Loads retrieved successfully"));
    }

    /// <summary>
    /// Get all loads with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of loads</returns>
    [HttpGet]
    [RequirePermission("LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<LoadDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching loads - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new LoadSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetLoadsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<LoadDto>>()
            .SetSuccess(result, result.TotalCount, "Loads retrieved successfully"));
    }

    /// <summary>
    /// Get a load by its unique ID
    /// </summary>
    /// <param name="id">Load ID (bigint)</param>
    /// <returns>Load details</returns>
    [HttpGet("{id}")]
    [RequirePermission("LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<LoadDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching load with id: {LoadId}", User.Identity?.Name, id);

        var query = new GetLoadByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<LoadDto>()
                .SetError(404, $"Load with id {id} not found"));
        }

        return Ok(new APIBaseResponse<LoadDto>()
            .SetSuccess(result, "Load retrieved successfully"));
    }

    /// <summary>
    /// Get all loads for a specific load request
    /// </summary>
    /// <param name="loadRequestId">Load request ID (bigint)</param>
    /// <returns>List of loads</returns>
    [HttpGet("by-load-request/{loadRequestId}")]
    [RequirePermission("LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<LoadDto>>>> GetByLoadRequestId([FromRoute] long loadRequestId)
    {
        _logger.LogInformation("User {User} fetching loads for load request: {LoadRequestId}", User.Identity?.Name, loadRequestId);

        var query = new GetLoadsByLoadRequestIdQuery(loadRequestId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<LoadDto>>()
            .SetSuccess(result, "Loads for load request retrieved successfully"));
    }

    /// <summary>
    /// Get all loads performed by or assigned to a specific driver
    /// </summary>
    /// <param name="driverId">Driver user ID</param>
    /// <returns>List of loads</returns>
    [HttpGet("by-driver/{driverId}")]
    [RequirePermission("LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<LoadDto>>>> GetByDriverId([FromRoute] int driverId)
    {
        _logger.LogInformation("User {User} fetching loads for driver: {DriverId}", User.Identity?.Name, driverId);

        var query = new GetLoadsByDriverIdQuery(driverId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<LoadDto>>()
            .SetSuccess(result, "Loads for driver retrieved successfully"));
    }

    /// <summary>
    /// Create a batch / list of loads (used by Mobile App)
    /// </summary>
    /// <param name="command">Create loads command containing a list of load items</param>
    /// <returns>List of created loads</returns>
    [HttpPost]
    [RequirePermission("LOADS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<List<LoadDto>>>> CreateBatch(
        [FromBody] CreateLoadsCommand command)
    {
        _logger.LogInformation("User {User} creating batch of {Count} loads", User.Identity?.Name, command?.Loads?.Count ?? 0);

        var result = await _mediator.Send(command);

        return StatusCode(StatusCodes.Status201Created,
            new APIBaseResponse<List<LoadDto>>()
                .SetSuccess(result, result.Count, "Loads created successfully"));
    }

    /// <summary>
    /// Update a batch / list of loads (used by Mobile App)
    /// </summary>
    /// <param name="command">Update loads command containing a list of updated load items</param>
    /// <returns>List of updated loads</returns>
    [HttpPut]
    [RequirePermission("LOADS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<List<LoadDto>>>> UpdateBatch(
        [FromBody] UpdateLoadsCommand command)
    {
        _logger.LogInformation("User {User} updating batch of {Count} loads", User.Identity?.Name, command?.Loads?.Count ?? 0);

        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<List<LoadDto>>()
            .SetSuccess(result, result.Count, "Loads updated successfully"));
    }

    /// <summary>
    /// Update a single load by ID
    /// </summary>
    /// <param name="id">Load ID (bigint)</param>
    /// <param name="item">Load update data</param>
    /// <returns>Updated load details</returns>
    [HttpPut("{id}")]
    [RequirePermission("LOADS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<LoadDto>>> UpdateSingle(
        [FromRoute] long id,
        [FromBody] UpdateLoadItemDto item)
    {
        _logger.LogInformation("User {User} updating load with id: {LoadId}", User.Identity?.Name, id);

        item.Id = id;
        var command = new UpdateLoadsCommand(new List<UpdateLoadItemDto> { item });
        var result = await _mediator.Send(command);

        var updated = result.FirstOrDefault();
        if (updated == null)
        {
            return NotFound(new APIBaseResponse<LoadDto>()
                .SetError(404, $"Load with id {id} not found"));
        }

        return Ok(new APIBaseResponse<LoadDto>()
            .SetSuccess(updated, "Load updated successfully"));
    }

    /// <summary>
    /// Delete a batch / list of loads by their IDs (used by Mobile App)
    /// </summary>
    /// <param name="loadIds">List of load IDs to delete</param>
    /// <returns>Success status</returns>
    [HttpDelete]
    [RequirePermission("LOADS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> DeleteBatch(
        [FromBody] List<long> loadIds)
    {
        _logger.LogInformation("User {User} deleting batch of {Count} loads", User.Identity?.Name, loadIds?.Count ?? 0);

        var command = new DeleteLoadsCommand(loadIds ?? new List<long>());
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, "No loads found to delete"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Loads deleted successfully"));
    }

    /// <summary>
    /// Alternative endpoint to delete a batch / list of loads (useful for HTTP clients with DELETE body limitations)
    /// </summary>
    /// <param name="command">Delete loads command with list of load IDs</param>
    /// <returns>Success status</returns>
    [HttpPost("batch-delete")]
    [RequirePermission("LOADS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> BatchDeletePost(
        [FromBody] DeleteLoadsCommand command)
    {
        _logger.LogInformation("User {User} batch-deleting {Count} loads via POST", User.Identity?.Name, command?.LoadIds?.Count ?? 0);

        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, "No loads found to delete"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Loads deleted successfully"));
    }

    /// <summary>
    /// Delete a single load by ID
    /// </summary>
    /// <param name="id">Load ID (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("LOADS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> DeleteSingle([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting load with id: {LoadId}", User.Identity?.Name, id);

        var command = new DeleteLoadsCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Load with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Load deleted successfully"));
    }
}
