using CleanSample.Application.Commands.ClientLocation;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.ClientLocation;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for ClientLocation operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientLocationsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ClientLocationsController> _logger;

    public ClientLocationsController(IMediator mediator, ILogger<ClientLocationsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all client locations with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Client location search filter object</param>
    /// <returns>Paginated list of client locations</returns>
    [HttpPost("search")]
    [RequirePermission("CLIENT_LOCATIONS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ClientLocationDto>>>> Search(
        [FromBody] ClientLocationSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching client locations", User.Identity?.Name);

        var query = new GetClientLocationsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ClientLocationDto>>()
            .SetSuccess(result, result.TotalCount, "Client locations retrieved successfully"));
    }

    /// <summary>
    /// Get all client locations with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of client locations</returns>
    [HttpGet]
    [RequirePermission("CLIENT_LOCATIONS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ClientLocationDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching client locations - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new ClientLocationSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetClientLocationsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ClientLocationDto>>()
            .SetSuccess(result, result.TotalCount, "Client locations retrieved successfully"));
    }

    /// <summary>
    /// Get client location by id
    /// </summary>
    /// <param name="id">Client location id</param>
    /// <returns>Client location details</returns>
    [HttpGet("{id}")]
    [RequirePermission("CLIENT_LOCATIONS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<ClientLocationDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching client location with id: {ClientLocationId}", User.Identity?.Name, id);

        var query = new GetClientLocationByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<ClientLocationDto>()
                .SetError(404, $"Client location with id {id} not found"));
        }

        return Ok(new APIBaseResponse<ClientLocationDto>()
            .SetSuccess(result, "Client location retrieved successfully"));
    }

    /// <summary>
    /// Get all locations for a specific client
    /// </summary>
    /// <param name="clientId">Client id</param>
    /// <returns>List of client locations</returns>
    [HttpGet("client/{clientId}")]
    [RequirePermission("CLIENT_LOCATIONS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<ClientLocationDto>>>> GetByClientId([FromRoute] int clientId)
    {
        _logger.LogInformation("User {User} fetching locations for client: {ClientId}", User.Identity?.Name, clientId);

        var query = new GetClientLocationsByClientIdQuery(clientId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<ClientLocationDto>>()
            .SetSuccess(result, "Client locations retrieved successfully"));
    }

    /// <summary>
    /// Create or update client locations (supports single item or list of items)
    /// </summary>
    /// <param name="command">Client location creation/update command</param>
    /// <returns>List of created/updated client locations</returns>
    [HttpPost]
    [RequirePermission("CLIENT_LOCATIONS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<List<ClientLocationDto>>>> Create([FromBody] CreateClientLocationCommand command)
    {
        _logger.LogInformation("User {User} creating/updating client locations", User.Identity?.Name);

        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<List<ClientLocationDto>>()
            .SetSuccess(result, result.Count, "Client locations processed successfully"));
    }

    /// <summary>
    /// Batch create or update client locations from a list of items
    /// </summary>
    /// <param name="items">List of client location items to create or update</param>
    /// <returns>List of created/updated client locations</returns>
    [HttpPost("batch")]
    [RequirePermission("CLIENT_LOCATIONS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<APIBaseResponse<List<ClientLocationDto>>>> Batch([FromBody] List<CreateClientLocationItemDto> items)
    {
        _logger.LogInformation("User {User} batch creating/updating {Count} client locations", User.Identity?.Name, items?.Count ?? 0);

        var command = new CreateClientLocationCommand(items ?? new List<CreateClientLocationItemDto>());
        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<List<ClientLocationDto>>()
            .SetSuccess(result, result.Count, "Client locations processed successfully"));
    }

    /// <summary>
    /// Update an existing client location
    /// </summary>
    /// <param name="id">Client location id</param>
    /// <param name="command">Client location update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("CLIENT_LOCATIONS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] int id,
        [FromBody] UpdateClientLocationCommand command)
    {
        _logger.LogInformation("User {User} updating client location with id: {ClientLocationId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Client location with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Client location updated successfully"));
    }

    /// <summary>
    /// Delete a client location
    /// </summary>
    /// <param name="id">Client location id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("CLIENT_LOCATIONS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] int id)
    {
        _logger.LogInformation("User {User} deleting client location with id: {ClientLocationId}", User.Identity?.Name, id);

        var command = new DeleteClientLocationCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Client location with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Client location deleted successfully"));
    }
}
