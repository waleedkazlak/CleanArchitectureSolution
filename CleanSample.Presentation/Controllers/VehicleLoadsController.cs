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
/// API Controller for Mobile App Vehicle Load operations (accepts list of records)
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
    /// Create a batch / list of vehicle loads (used by Mobile App)
    /// </summary>
    /// <param name="items">List of vehicle load items to create</param>
    /// <returns>List of created vehicle loads</returns>
    [HttpPost]
    [RequirePermission("VEHICLE_LOADS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<List<VehicleLoadDto>>>> Create(
        [FromBody] List<CreateVehicleLoadItemDto> items)
    {
        _logger.LogInformation("User {User} creating batch of {Count} vehicle loads", User.Identity?.Name, items?.Count ?? 0);

        var command = new CreateVehicleLoadsCommand(items ?? new List<CreateVehicleLoadItemDto>());
        var result = await _mediator.Send(command);

        return StatusCode(StatusCodes.Status201Created,
            new APIBaseResponse<List<VehicleLoadDto>>()
                .SetSuccess(result, result.Count, "Vehicle loads created successfully"));
    }
}
