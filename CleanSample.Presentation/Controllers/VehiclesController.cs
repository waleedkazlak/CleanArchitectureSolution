using CleanSample.Application.Commands.Vehicle;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Vehicle;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Vehicle operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<VehiclesController> _logger;

    public VehiclesController(IMediator mediator, ILogger<VehiclesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all vehicles with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Vehicle search filter object</param>
    /// <returns>Paginated list of vehicles</returns>
    [HttpPost("search")]
    [RequirePermission("VEHICLES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<VehicleDto>>>> Search(
        [FromBody] VehicleSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching vehicles", User.Identity?.Name);

        var query = new GetVehiclesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<VehicleDto>>()
            .SetSuccess(result, result.TotalCount, "Vehicles retrieved successfully"));
    }

    /// <summary>
    /// Get all vehicles with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of vehicles</returns>
    [HttpGet]
    [RequirePermission("VEHICLES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<VehicleDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching vehicles - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new VehicleSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetVehiclesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<VehicleDto>>()
            .SetSuccess(result, result.TotalCount, "Vehicles retrieved successfully"));
    }

    /// <summary>
    /// Get vehicle by id
    /// </summary>
    /// <param name="id">Vehicle id</param>
    /// <returns>Vehicle details</returns>
    [HttpGet("{id}")]
    [RequirePermission("VEHICLES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<VehicleDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching vehicle with id: {VehicleId}", User.Identity?.Name, id);

        var query = new GetVehicleByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<VehicleDto>()
                .SetError(404, $"Vehicle with id {id} not found"));
        }

        return Ok(new APIBaseResponse<VehicleDto>()
            .SetSuccess(result, "Vehicle retrieved successfully"));
    }

    /// <summary>
    /// Create a new vehicle
    /// </summary>
    /// <param name="command">Vehicle creation command</param>
    /// <returns>Created vehicle id</returns>
    [HttpPost]
    [RequirePermission("VEHICLES", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateVehicleCommand command)
    {
        _logger.LogInformation("User {User} creating new vehicle: {VehicleNumber}", User.Identity?.Name, command?.VehicleNumber);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<int>()
                .SetSuccess(result, "Vehicle created successfully"));
    }

    /// <summary>
    /// Update an existing vehicle
    /// </summary>
    /// <param name="id">Vehicle id</param>
    /// <param name="command">Vehicle update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("VEHICLES", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] int id,
        [FromBody] UpdateVehicleCommand command)
    {
        _logger.LogInformation("User {User} updating vehicle with id: {VehicleId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Vehicle with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Vehicle updated successfully"));
    }

    /// <summary>
    /// Delete a vehicle
    /// </summary>
    /// <param name="id">Vehicle id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("VEHICLES", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] int id)
    {
        _logger.LogInformation("User {User} deleting vehicle with id: {VehicleId}", User.Identity?.Name, id);

        var command = new DeleteVehicleCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Vehicle with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Vehicle deleted successfully"));
    }
}
