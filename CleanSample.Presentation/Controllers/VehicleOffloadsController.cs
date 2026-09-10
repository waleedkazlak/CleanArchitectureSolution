using CleanSample.Application.Commands.VehicleOffload;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.VehicleOffload;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for unified VehicleOffload and VehicleOffloadItems operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehicleOffloadsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<VehicleOffloadsController> _logger;

    public VehicleOffloadsController(IMediator mediator, ILogger<VehicleOffloadsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Search and filter vehicle offloads with pagination
    /// </summary>
    /// <param name="filter">Vehicle offload search filter parameters</param>
    /// <returns>Paginated list of vehicle offloads</returns>
    [HttpPost("search")]
    [RequirePermission("VEHICLE_OFFLOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<VehicleOffloadDto>>>> Search(
        [FromBody] VehicleOffloadSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching vehicle offloads", User.Identity?.Name);

        var query = new GetVehicleOffloadsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<VehicleOffloadDto>>()
            .SetSuccess(result, result.TotalCount, "Vehicle offloads retrieved successfully"));
    }

    /// <summary>
    /// Get all vehicle offloads with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of vehicle offloads</returns>
    [HttpGet]
    [RequirePermission("VEHICLE_OFFLOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<VehicleOffloadDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching vehicle offloads - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new VehicleOffloadSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetVehicleOffloadsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<VehicleOffloadDto>>()
            .SetSuccess(result, result.TotalCount, "Vehicle offloads retrieved successfully"));
    }

    /// <summary>
    /// Get a vehicle offload by its unique ID (including all its items)
    /// </summary>
    /// <param name="id">Vehicle offload ID (bigint)</param>
    /// <returns>Vehicle offload details with all offload items</returns>
    [HttpGet("{id}")]
    [RequirePermission("VEHICLE_OFFLOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<VehicleOffloadDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching vehicle offload with id: {VehicleOffloadId}", User.Identity?.Name, id);

        var query = new GetVehicleOffloadByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<VehicleOffloadDto>()
                .SetError(404, $"Vehicle offload with id {id} not found"));
        }

        return Ok(new APIBaseResponse<VehicleOffloadDto>()
            .SetSuccess(result, "Vehicle offload retrieved successfully"));
    }

    /// <summary>
    /// Get all vehicle offloads for a specific load request
    /// </summary>
    /// <param name="loadRequestId">Load request ID (bigint)</param>
    /// <returns>List of vehicle offloads</returns>
    [HttpGet("by-load-request/{loadRequestId}")]
    [RequirePermission("VEHICLE_OFFLOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<VehicleOffloadDto>>>> GetByLoadRequestId([FromRoute] long loadRequestId)
    {
        _logger.LogInformation("User {User} fetching vehicle offloads for load request: {LoadRequestId}", User.Identity?.Name, loadRequestId);

        var query = new GetVehicleOffloadsByLoadRequestIdQuery(loadRequestId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<VehicleOffloadDto>>()
            .SetSuccess(result, "Vehicle offloads for load request retrieved successfully"));
    }

    /// <summary>
    /// Get all vehicle offloads for a specific driver
    /// </summary>
    /// <param name="driverId">Driver user ID</param>
    /// <returns>List of vehicle offloads</returns>
    [HttpGet("by-driver/{driverId}")]
    [RequirePermission("VEHICLE_OFFLOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<VehicleOffloadDto>>>> GetByDriverId([FromRoute] int driverId)
    {
        _logger.LogInformation("User {User} fetching vehicle offloads for driver: {DriverId}", User.Identity?.Name, driverId);

        var query = new GetVehicleOffloadsByDriverIdQuery(driverId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<VehicleOffloadDto>>()
            .SetSuccess(result, "Vehicle offloads for driver retrieved successfully"));
    }

    /// <summary>
    /// Get all vehicle offloads for a specific vehicle
    /// </summary>
    /// <param name="vehicleId">Vehicle ID</param>
    /// <returns>List of vehicle offloads</returns>
    [HttpGet("by-vehicle/{vehicleId}")]
    [RequirePermission("VEHICLE_OFFLOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<VehicleOffloadDto>>>> GetByVehicleId([FromRoute] int vehicleId)
    {
        _logger.LogInformation("User {User} fetching vehicle offloads for vehicle: {VehicleId}", User.Identity?.Name, vehicleId);

        var query = new GetVehicleOffloadsByVehicleIdQuery(vehicleId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<VehicleOffloadDto>>()
            .SetSuccess(result, "Vehicle offloads for vehicle retrieved successfully"));
    }

    /// <summary>
    /// Create a whole vehicle offload and its related vehicle offload items in a single API call
    /// </summary>
    /// <param name="command">Vehicle offload creation command with items</param>
    /// <returns>Created vehicle offload ID</returns>
    [HttpPost]
    [RequirePermission("VEHICLE_OFFLOADS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<long>>> Create(
        [FromBody] CreateVehicleOffloadCommand command)
    {
        _logger.LogInformation("User {User} creating vehicle offload for LoadRequestId: {LoadRequestId}",
            User.Identity?.Name, command?.LoadRequestId);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<long>()
                .SetSuccess(result, "Vehicle offload created successfully"));
    }

    /// <summary>
    /// Update a whole vehicle offload along with all its related vehicle offload items
    /// </summary>
    /// <param name="id">Vehicle offload ID (bigint)</param>
    /// <param name="command">Vehicle offload update command with items</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("VEHICLE_OFFLOADS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] long id,
        [FromBody] UpdateVehicleOffloadCommand command)
    {
        _logger.LogInformation("User {User} updating vehicle offload with id: {VehicleOffloadId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Vehicle offload with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Vehicle offload updated successfully"));
    }

    /// <summary>
    /// Delete a whole vehicle offload and its related vehicle offload items
    /// </summary>
    /// <param name="id">Vehicle offload ID (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("VEHICLE_OFFLOADS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting vehicle offload with id: {VehicleOffloadId}", User.Identity?.Name, id);

        var command = new DeleteVehicleOffloadCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Vehicle offload with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Vehicle offload deleted successfully"));
    }
}
