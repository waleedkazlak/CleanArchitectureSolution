using CleanSample.Application.Commands.Client;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Client operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ClientsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(IMediator mediator, ILogger<ClientsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all clients with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Client search filter object</param>
    /// <returns>Paginated list of clients</returns>
    [HttpPost("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ClientDto>>>> Search(
        [FromBody] ClientSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching clients", User.Identity?.Name);

        var query = new GetClientsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ClientDto>>()
            .SetSuccess(result, result.TotalCount, "Clients retrieved successfully"));
    }

    /// <summary>
    /// Get all clients with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of clients</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ClientDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching clients - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new ClientSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetClientsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ClientDto>>()
            .SetSuccess(result, result.TotalCount, "Clients retrieved successfully"));
    }

    /// <summary>
    /// Get client by id
    /// </summary>
    /// <param name="id">Client id</param>
    /// <returns>Client details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<ClientDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching client with id: {ClientId}", User.Identity?.Name, id);

        var query = new GetClientByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<ClientDto>()
                .SetError(404, $"Client with id {id} not found"));
        }

        return Ok(new APIBaseResponse<ClientDto>()
            .SetSuccess(result, "Client retrieved successfully"));
    }

    /// <summary>
    /// Create a new client (Admin only)
    /// </summary>
    /// <param name="command">Client creation command</param>
    /// <returns>Created client id</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateClientCommand command)
    {
        _logger.LogInformation("User {User} creating new client: {ClientName}", User.Identity?.Name, command?.Name);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<int>()
                .SetSuccess(result, "Client created successfully"));
    }

    /// <summary>
    /// Update an existing client (Admin only)
    /// </summary>
    /// <param name="id">Client id</param>
    /// <param name="command">Client update command</param>
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
        [FromBody] UpdateClientCommand command)
    {
        _logger.LogInformation("User {User} updating client with id: {ClientId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Client with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Client updated successfully"));
    }

    /// <summary>
    /// Delete a client (Admin only)
    /// </summary>
    /// <param name="id">Client id</param>
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
        _logger.LogInformation("User {User} deleting client with id: {ClientId}", User.Identity?.Name, id);

        var command = new DeleteClientCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Client with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Client deleted successfully"));
    }
}
