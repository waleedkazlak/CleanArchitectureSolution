using CleanSample.Application.Commands.Pick;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Pick;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Mobile App Pick operations (supports batch/list CRUD operations)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PicksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PicksController> _logger;

    public PicksController(IMediator mediator, ILogger<PicksController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Search and filter picks with pagination
    /// </summary>
    /// <param name="filter">Pick search filter parameters</param>
    /// <returns>Paginated list of picks</returns>
    [HttpPost("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<PickDto>>>> Search(
        [FromBody] PickSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching picks", User.Identity?.Name);

        var query = new GetPicksWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<PickDto>>()
            .SetSuccess(result, result.TotalCount, "Picks retrieved successfully"));
    }

    /// <summary>
    /// Get all picks with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of picks</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<PickDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching picks - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new PickSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetPicksWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<PickDto>>()
            .SetSuccess(result, result.TotalCount, "Picks retrieved successfully"));
    }

    /// <summary>
    /// Get a pick by its unique ID
    /// </summary>
    /// <param name="id">Pick ID (bigint)</param>
    /// <returns>Pick details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PickDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching pick with id: {PickId}", User.Identity?.Name, id);

        var query = new GetPickByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<PickDto>()
                .SetError(404, $"Pick with id {id} not found"));
        }

        return Ok(new APIBaseResponse<PickDto>()
            .SetSuccess(result, "Pick retrieved successfully"));
    }

    /// <summary>
    /// Get all picks for a specific pick request
    /// </summary>
    /// <param name="pickRequestId">Pick request ID (bigint)</param>
    /// <returns>List of picks</returns>
    [HttpGet("by-pick-request/{pickRequestId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<PickDto>>>> GetByPickRequestId([FromRoute] long pickRequestId)
    {
        _logger.LogInformation("User {User} fetching picks for pick request: {PickRequestId}", User.Identity?.Name, pickRequestId);

        var query = new GetPicksByPickRequestIdQuery(pickRequestId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<PickDto>>()
            .SetSuccess(result, "Picks for pick request retrieved successfully"));
    }

    /// <summary>
    /// Get all picks performed by or assigned to a specific driver
    /// </summary>
    /// <param name="driverId">Driver user ID</param>
    /// <returns>List of picks</returns>
    [HttpGet("by-driver/{driverId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<PickDto>>>> GetByDriverId([FromRoute] int driverId)
    {
        _logger.LogInformation("User {User} fetching picks for driver: {DriverId}", User.Identity?.Name, driverId);

        var query = new GetPicksByDriverIdQuery(driverId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<PickDto>>()
            .SetSuccess(result, "Picks for driver retrieved successfully"));
    }

    /// <summary>
    /// Create a batch / list of picks (used by Mobile App)
    /// </summary>
    /// <param name="command">Create picks command containing a list of pick items</param>
    /// <returns>List of created picks</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<List<PickDto>>>> CreateBatch(
        [FromBody] CreatePicksCommand command)
    {
        _logger.LogInformation("User {User} creating batch of {Count} picks", User.Identity?.Name, command?.Picks?.Count ?? 0);

        var result = await _mediator.Send(command);

        return StatusCode(StatusCodes.Status201Created,
            new APIBaseResponse<List<PickDto>>()
                .SetSuccess(result, result.Count, "Picks created successfully"));
    }

    /// <summary>
    /// Update a batch / list of picks (used by Mobile App)
    /// </summary>
    /// <param name="command">Update picks command containing a list of updated pick items</param>
    /// <returns>List of updated picks</returns>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<List<PickDto>>>> UpdateBatch(
        [FromBody] UpdatePicksCommand command)
    {
        _logger.LogInformation("User {User} updating batch of {Count} picks", User.Identity?.Name, command?.Picks?.Count ?? 0);

        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<List<PickDto>>()
            .SetSuccess(result, result.Count, "Picks updated successfully"));
    }

    /// <summary>
    /// Update a single pick by ID
    /// </summary>
    /// <param name="id">Pick ID (bigint)</param>
    /// <param name="item">Pick update data</param>
    /// <returns>Updated pick details</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PickDto>>> UpdateSingle(
        [FromRoute] long id,
        [FromBody] UpdatePickItemDto item)
    {
        _logger.LogInformation("User {User} updating pick with id: {PickId}", User.Identity?.Name, id);

        item.Id = id;
        var command = new UpdatePicksCommand(new List<UpdatePickItemDto> { item });
        var result = await _mediator.Send(command);

        var updated = result.FirstOrDefault();
        if (updated == null)
        {
            return NotFound(new APIBaseResponse<PickDto>()
                .SetError(404, $"Pick with id {id} not found"));
        }

        return Ok(new APIBaseResponse<PickDto>()
            .SetSuccess(updated, "Pick updated successfully"));
    }

    /// <summary>
    /// Delete a batch / list of picks by their IDs (used by Mobile App)
    /// </summary>
    /// <param name="pickIds">List of pick IDs to delete</param>
    /// <returns>Success status</returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> DeleteBatch(
        [FromBody] List<long> pickIds)
    {
        _logger.LogInformation("User {User} deleting batch of {Count} picks", User.Identity?.Name, pickIds?.Count ?? 0);

        var command = new DeletePicksCommand(pickIds ?? new List<long>());
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, "No picks found to delete"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Picks deleted successfully"));
    }

    /// <summary>
    /// Alternative endpoint to delete a batch / list of picks (useful for HTTP clients with DELETE body limitations)
    /// </summary>
    /// <param name="command">Delete picks command with list of pick IDs</param>
    /// <returns>Success status</returns>
    [HttpPost("batch-delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> BatchDeletePost(
        [FromBody] DeletePicksCommand command)
    {
        _logger.LogInformation("User {User} batch-deleting {Count} picks via POST", User.Identity?.Name, command?.PickIds?.Count ?? 0);

        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, "No picks found to delete"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Picks deleted successfully"));
    }

    /// <summary>
    /// Delete a single pick by ID
    /// </summary>
    /// <param name="id">Pick ID (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> DeleteSingle([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting pick with id: {PickId}", User.Identity?.Name, id);

        var command = new DeletePicksCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Pick with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Pick deleted successfully"));
    }
}
