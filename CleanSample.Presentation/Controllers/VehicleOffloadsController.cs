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
/// API Controller for Mobile App Vehicle Offload operations (accepts list of records)
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
    /// Create a batch / list of vehicle offloads (used by Mobile App)
    /// </summary>
    /// <param name="items">List of vehicle offload items to create</param>
    /// <returns>List of created vehicle offloads</returns>
    [HttpPost]
    [RequirePermission("VEHICLE_OFFLOADS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<List<VehicleOffloadDto>>>> Create(
        [FromBody] List<CreateVehicleOffloadItemDto> items)
    {
        _logger.LogInformation("User {User} creating batch of {Count} vehicle offloads", User.Identity?.Name, items?.Count ?? 0);

        var command = new CreateVehicleOffloadsCommand(items ?? new List<CreateVehicleOffloadItemDto>());
        var result = await _mediator.Send(command);

        return StatusCode(StatusCodes.Status201Created,
            new APIBaseResponse<List<VehicleOffloadDto>>()
                .SetSuccess(result, result.Count, "Vehicle offloads created successfully"));
    }
}
