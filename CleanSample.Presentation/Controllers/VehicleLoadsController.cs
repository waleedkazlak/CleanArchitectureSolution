using CleanSample.Application.Commands.VehicleLoad;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.VehicleLoad;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for unified VehicleLoad and VehicleLoadItems operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehicleLoadsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<VehicleLoadsController> _logger;

    public VehicleLoadsController(IMediator mediator, ILogger<VehicleLoadsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Search and filter vehicle loads with pagination
    /// </summary>
    /// <param name="filter">Vehicle load search filter parameters</param>
    /// <returns>Paginated list of vehicle loads</returns>
    [HttpPost("search")]
    [RequirePermission("VEHICLE_LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<VehicleLoadDto>>>> Search(
        [FromBody] VehicleLoadSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching vehicle loads", User.Identity?.Name);

        var query = new GetVehicleLoadsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<VehicleLoadDto>>()
            .SetSuccess(result, result.TotalCount, "Vehicle loads retrieved successfully"));
    }

    /// <summary>
    /// Get all vehicle loads with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of vehicle loads</returns>
    [HttpGet]
    [RequirePermission("VEHICLE_LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<VehicleLoadDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching vehicle loads - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new VehicleLoadSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetVehicleLoadsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<VehicleLoadDto>>()
            .SetSuccess(result, result.TotalCount, "Vehicle loads retrieved successfully"));
    }

    /// <summary>
    /// Get a vehicle load by its unique ID (including all its items)
    /// </summary>
    /// <param name="id">Vehicle load ID (bigint)</param>
    /// <returns>Vehicle load details with all load items</returns>
    [HttpGet("{id}")]
    [RequirePermission("VEHICLE_LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<VehicleLoadDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching vehicle load with id: {VehicleLoadId}", User.Identity?.Name, id);

        var query = new GetVehicleLoadByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<VehicleLoadDto>()
                .SetError(404, $"Vehicle load with id {id} not found"));
        }

        return Ok(new APIBaseResponse<VehicleLoadDto>()
            .SetSuccess(result, "Vehicle load retrieved successfully"));
    }

    /// <summary>
    /// Get all vehicle loads for a specific pick request
    /// </summary>
    /// <param name="pickRequestId">Pick request ID (bigint)</param>
    /// <returns>List of vehicle loads</returns>
    [HttpGet("by-pick-request/{pickRequestId}")]
    [RequirePermission("VEHICLE_LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<VehicleLoadDto>>>> GetByPickRequestId([FromRoute] long pickRequestId)
    {
        _logger.LogInformation("User {User} fetching vehicle loads for pick request: {PickRequestId}", User.Identity?.Name, pickRequestId);

        var query = new GetVehicleLoadsByPickRequestIdQuery(pickRequestId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<VehicleLoadDto>>()
            .SetSuccess(result, "Vehicle loads for pick request retrieved successfully"));
    }

    /// <summary>
    /// Get all vehicle loads for a specific driver
    /// </summary>
    /// <param name="driverId">Driver user ID</param>
    /// <returns>List of vehicle loads</returns>
    [HttpGet("by-driver/{driverId}")]
    [RequirePermission("VEHICLE_LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<VehicleLoadDto>>>> GetByDriverId([FromRoute] int driverId)
    {
        _logger.LogInformation("User {User} fetching vehicle loads for driver: {DriverId}", User.Identity?.Name, driverId);

        var query = new GetVehicleLoadsByDriverIdQuery(driverId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<VehicleLoadDto>>()
            .SetSuccess(result, "Vehicle loads for driver retrieved successfully"));
    }

    /// <summary>
    /// Get all vehicle loads for a specific vehicle
    /// </summary>
    /// <param name="vehicleId">Vehicle ID</param>
    /// <returns>List of vehicle loads</returns>
    [HttpGet("by-vehicle/{vehicleId}")]
    [RequirePermission("VEHICLE_LOADS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<VehicleLoadDto>>>> GetByVehicleId([FromRoute] int vehicleId)
    {
        _logger.LogInformation("User {User} fetching vehicle loads for vehicle: {VehicleId}", User.Identity?.Name, vehicleId);

        var query = new GetVehicleLoadsByVehicleIdQuery(vehicleId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<VehicleLoadDto>>()
            .SetSuccess(result, "Vehicle loads for vehicle retrieved successfully"));
    }

    /// <summary>
    /// Create a whole vehicle load and its related vehicle load items in a single API call
    /// </summary>
    /// <param name="command">Vehicle load creation command with items</param>
    /// <returns>Created vehicle load ID</returns>
    [HttpPost]
    [RequirePermission("VEHICLE_LOADS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<long>>> Create(
        [FromBody] CreateVehicleLoadCommand command)
    {
        _logger.LogInformation("User {User} creating vehicle load for PickRequestId: {PickRequestId}",
            User.Identity?.Name, command?.PickRequestId);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<long>()
                .SetSuccess(result, "Vehicle load created successfully"));
    }

    /// <summary>
    /// Update a whole vehicle load along with all its related vehicle load items
    /// </summary>
    /// <param name="id">Vehicle load ID (bigint)</param>
    /// <param name="command">Vehicle load update command with items</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("VEHICLE_LOADS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] long id,
        [FromBody] UpdateVehicleLoadCommand command)
    {
        _logger.LogInformation("User {User} updating vehicle load with id: {VehicleLoadId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Vehicle load with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Vehicle load updated successfully"));
    }

    /// <summary>
    /// Delete a whole vehicle load and its related vehicle load items
    /// </summary>
    /// <param name="id">Vehicle load ID (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("VEHICLE_LOADS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting vehicle load with id: {VehicleLoadId}", User.Identity?.Name, id);

        var command = new DeleteVehicleLoadCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Vehicle load with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Vehicle load deleted successfully"));
    }
}
