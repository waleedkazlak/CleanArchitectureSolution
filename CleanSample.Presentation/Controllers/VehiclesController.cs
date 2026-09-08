using CleanSample.Application.Commands.Vehicle;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Vehicle;
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
    /// Get all vehicles
    /// </summary>
    /// <returns>List of vehicles</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<VehicleDto>>>> GetAll()
    {
        _logger.LogInformation("User {User} fetching vehicles", User.Identity?.Name);

        var query = new GetAllVehiclesQuery();
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<VehicleDto>>()
            .SetSuccess(result, result.Count(), "Vehicles retrieved successfully"));
    }

    /// <summary>
    /// Get vehicle by id
    /// </summary>
    /// <param name="id">Vehicle id</param>
    /// <returns>Vehicle details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
    /// Create a new vehicle (Admin only)
    /// </summary>
    /// <param name="command">Vehicle creation command</param>
    /// <returns>Created vehicle id</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
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
    /// Update an existing vehicle (Admin only)
    /// </summary>
    /// <param name="id">Vehicle id</param>
    /// <param name="command">Vehicle update command</param>
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
    /// Delete a vehicle (Admin only)
    /// </summary>
    /// <param name="id">Vehicle id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
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
