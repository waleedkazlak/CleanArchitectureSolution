using CleanSample.Application.Commands.FieldAssembly;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.FieldAssembly;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for FieldAssembly operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FieldAssembliesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<FieldAssembliesController> _logger;

    public FieldAssembliesController(IMediator mediator, ILogger<FieldAssembliesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Search and filter field assemblies with pagination
    /// </summary>
    /// <param name="filter">Field assembly search filter parameters</param>
    /// <returns>Paginated list of field assemblies</returns>
    [HttpPost("search")]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<FieldAssemblyDto>>>> Search(
        [FromBody] FieldAssemblySearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching field assemblies", User.Identity?.Name);

        var query = new GetFieldAssembliesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<FieldAssemblyDto>>()
            .SetSuccess(result, result.TotalCount, "Field assemblies retrieved successfully"));
    }

    /// <summary>
    /// Get all field assemblies with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of field assemblies</returns>
    [HttpGet]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<FieldAssemblyDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching field assemblies - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new FieldAssemblySearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetFieldAssembliesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<FieldAssemblyDto>>()
            .SetSuccess(result, result.TotalCount, "Field assemblies retrieved successfully"));
    }

    /// <summary>
    /// Get a field assembly by its unique ID
    /// </summary>
    /// <param name="id">Field assembly ID (bigint)</param>
    /// <returns>Field assembly details</returns>
    [HttpGet("{id}")]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<FieldAssemblyDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching field assembly with id: {FieldAssemblyId}", User.Identity?.Name, id);

        var query = new GetFieldAssemblyByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<FieldAssemblyDto>()
                .SetError(404, $"Field assembly with id {id} not found"));
        }

        return Ok(new APIBaseResponse<FieldAssemblyDto>()
            .SetSuccess(result, "Field assembly retrieved successfully"));
    }

    /// <summary>
    /// Get all field assemblies for a specific field job
    /// </summary>
    /// <param name="fieldJobId">Field job ID (bigint)</param>
    /// <returns>List of field assemblies</returns>
    [HttpGet("by-field-job/{fieldJobId}")]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<FieldAssemblyDto>>>> GetByFieldJobId([FromRoute] long fieldJobId)
    {
        _logger.LogInformation("User {User} fetching field assemblies for field job: {FieldJobId}", User.Identity?.Name, fieldJobId);

        var query = new GetFieldAssembliesByFieldJobIdQuery(fieldJobId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<FieldAssemblyDto>>()
            .SetSuccess(result, "Field assemblies for field job retrieved successfully"));
    }

    /// <summary>
    /// Get all field assemblies for a specific product
    /// </summary>
    /// <param name="productId">Product ID</param>
    /// <returns>List of field assemblies</returns>
    [HttpGet("by-product/{productId}")]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<FieldAssemblyDto>>>> GetByProductId([FromRoute] int productId)
    {
        _logger.LogInformation("User {User} fetching field assemblies for product: {ProductId}", User.Identity?.Name, productId);

        var query = new GetFieldAssembliesByProductIdQuery(productId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<FieldAssemblyDto>>()
            .SetSuccess(result, "Field assemblies for product retrieved successfully"));
    }

    /// <summary>
    /// Get all field assemblies for a specific technician
    /// </summary>
    /// <param name="technicianId">Technician user ID</param>
    /// <returns>List of field assemblies</returns>
    [HttpGet("by-technician/{technicianId}")]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<FieldAssemblyDto>>>> GetByTechnicianId([FromRoute] int technicianId)
    {
        _logger.LogInformation("User {User} fetching field assemblies for technician: {TechnicianId}", User.Identity?.Name, technicianId);

        var query = new GetFieldAssembliesByTechnicianIdQuery(technicianId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<FieldAssemblyDto>>()
            .SetSuccess(result, "Field assemblies for technician retrieved successfully"));
    }

    /// <summary>
    /// Get all field assemblies for a specific supervisor
    /// </summary>
    /// <param name="supervisorId">Supervisor user ID</param>
    /// <returns>List of field assemblies</returns>
    [HttpGet("by-supervisor/{supervisorId}")]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<FieldAssemblyDto>>>> GetBySupervisorId([FromRoute] int supervisorId)
    {
        _logger.LogInformation("User {User} fetching field assemblies for supervisor: {SupervisorId}", User.Identity?.Name, supervisorId);

        var query = new GetFieldAssembliesBySupervisorIdQuery(supervisorId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<FieldAssemblyDto>>()
            .SetSuccess(result, "Field assemblies for supervisor retrieved successfully"));
    }

    /// <summary>
    /// Create or update field assembly records (supports single item, batch list, or command object)
    /// </summary>
    /// <param name="command">Field assembly creation command</param>
    /// <returns>Success status</returns>
    [HttpPost]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Create(
        [FromBody] CreateFieldAssemblyCommand command)
    {
        _logger.LogInformation("User {User} creating/updating field assemblies", User.Identity?.Name);

        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(result, "Field assemblies processed successfully"));
    }

    /// <summary>
    /// Batch create or update field assemblies from a list of items
    /// </summary>
    /// <param name="items">List of field assembly items to create or update</param>
    /// <returns>Success status</returns>
    [HttpPost("batch")]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Batch(
        [FromBody] List<CreateFieldAssemblyItemDto> items)
    {
        _logger.LogInformation("User {User} batch creating/updating {Count} field assemblies", User.Identity?.Name, items?.Count ?? 0);

        var command = new CreateFieldAssemblyCommand(items ?? new List<CreateFieldAssemblyItemDto>());
        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(result, "Field assemblies processed successfully"));
    }

    /// <summary>
    /// Update an existing field assembly
    /// </summary>
    /// <param name="id">Field assembly ID (bigint)</param>
    /// <param name="command">Field assembly update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] long id,
        [FromBody] UpdateFieldAssemblyCommand command)
    {
        _logger.LogInformation("User {User} updating field assembly with id: {FieldAssemblyId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Field assembly with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Field assembly updated successfully"));
    }

    /// <summary>
    /// Delete a field assembly
    /// </summary>
    /// <param name="id">Field assembly ID (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting field assembly with id: {FieldAssemblyId}", User.Identity?.Name, id);

        var command = new DeleteFieldAssemblyCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Field assembly with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Field assembly deleted successfully"));
    }
}
