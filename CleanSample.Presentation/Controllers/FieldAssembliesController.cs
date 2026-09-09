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
    /// Get all field assemblies for a specific product variant
    /// </summary>
    /// <param name="productVariantId">Product variant ID</param>
    /// <returns>List of field assemblies</returns>
    [HttpGet("by-product-variant/{productVariantId}")]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<FieldAssemblyDto>>>> GetByProductVariantId([FromRoute] int productVariantId)
    {
        _logger.LogInformation("User {User} fetching field assemblies for product variant: {ProductVariantId}", User.Identity?.Name, productVariantId);

        var query = new GetFieldAssembliesByProductVariantIdQuery(productVariantId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<FieldAssemblyDto>>()
            .SetSuccess(result, "Field assemblies for product variant retrieved successfully"));
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
    /// Create a new field assembly
    /// </summary>
    /// <param name="command">Field assembly creation command</param>
    /// <returns>Created field assembly ID</returns>
    [HttpPost]
    [RequirePermission("FIELD_ASSEMBLIES", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<long>>> Create(
        [FromBody] CreateFieldAssemblyCommand command)
    {
        _logger.LogInformation("User {User} creating field assembly for FieldJobId: {FieldJobId}",
            User.Identity?.Name, command?.FieldJobId);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<long>()
                .SetSuccess(result, "Field assembly created successfully"));
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
