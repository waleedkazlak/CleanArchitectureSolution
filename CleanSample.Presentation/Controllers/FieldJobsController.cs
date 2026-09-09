using CleanSample.Application.Commands.FieldJob;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.FieldJob;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for FieldJob operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FieldJobsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<FieldJobsController> _logger;

    public FieldJobsController(IMediator mediator, ILogger<FieldJobsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Search and filter field jobs with pagination
    /// </summary>
    /// <param name="filter">Field job search filter parameters</param>
    /// <returns>Paginated list of field jobs</returns>
    [HttpPost("search")]
    [RequirePermission("FIELD_JOBS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<FieldJobDto>>>> Search(
        [FromBody] FieldJobSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching field jobs", User.Identity?.Name);

        var query = new GetFieldJobsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<FieldJobDto>>()
            .SetSuccess(result, result.TotalCount, "Field jobs retrieved successfully"));
    }

    /// <summary>
    /// Get all field jobs with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of field jobs</returns>
    [HttpGet]
    [RequirePermission("FIELD_JOBS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<FieldJobDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching field jobs - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new FieldJobSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetFieldJobsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<FieldJobDto>>()
            .SetSuccess(result, result.TotalCount, "Field jobs retrieved successfully"));
    }

    /// <summary>
    /// Get a field job by its unique ID
    /// </summary>
    /// <param name="id">Field job ID (bigint)</param>
    /// <returns>Field job details</returns>
    [HttpGet("{id}")]
    [RequirePermission("FIELD_JOBS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<FieldJobDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching field job with id: {FieldJobId}", User.Identity?.Name, id);

        var query = new GetFieldJobByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<FieldJobDto>()
                .SetError(404, $"Field job with id {id} not found"));
        }

        return Ok(new APIBaseResponse<FieldJobDto>()
            .SetSuccess(result, "Field job retrieved successfully"));
    }

    /// <summary>
    /// Get all field jobs for a specific pick request
    /// </summary>
    /// <param name="pickRequestId">Pick request ID (bigint)</param>
    /// <returns>List of field jobs</returns>
    [HttpGet("by-pick-request/{pickRequestId}")]
    [RequirePermission("FIELD_JOBS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<FieldJobDto>>>> GetByPickRequestId([FromRoute] long pickRequestId)
    {
        _logger.LogInformation("User {User} fetching field jobs for pick request: {PickRequestId}", User.Identity?.Name, pickRequestId);

        var query = new GetFieldJobsByPickRequestIdQuery(pickRequestId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<FieldJobDto>>()
            .SetSuccess(result, "Field jobs for pick request retrieved successfully"));
    }

    /// <summary>
    /// Get all field jobs for a specific client
    /// </summary>
    /// <param name="clientId">Client ID</param>
    /// <returns>List of field jobs</returns>
    [HttpGet("by-client/{clientId}")]
    [RequirePermission("FIELD_JOBS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<FieldJobDto>>>> GetByClientId([FromRoute] int clientId)
    {
        _logger.LogInformation("User {User} fetching field jobs for client: {ClientId}", User.Identity?.Name, clientId);

        var query = new GetFieldJobsByClientIdQuery(clientId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<FieldJobDto>>()
            .SetSuccess(result, "Field jobs for client retrieved successfully"));
    }

    /// <summary>
    /// Get all field jobs for a specific technician
    /// </summary>
    /// <param name="technicianId">Technician user ID</param>
    /// <returns>List of field jobs</returns>
    [HttpGet("by-technician/{technicianId}")]
    [RequirePermission("FIELD_JOBS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<FieldJobDto>>>> GetByTechnicianId([FromRoute] int technicianId)
    {
        _logger.LogInformation("User {User} fetching field jobs for technician: {TechnicianId}", User.Identity?.Name, technicianId);

        var query = new GetFieldJobsByTechnicianIdQuery(technicianId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<FieldJobDto>>()
            .SetSuccess(result, "Field jobs for technician retrieved successfully"));
    }

    /// <summary>
    /// Get all field jobs for a specific supervisor
    /// </summary>
    /// <param name="supervisorId">Supervisor user ID</param>
    /// <returns>List of field jobs</returns>
    [HttpGet("by-supervisor/{supervisorId}")]
    [RequirePermission("FIELD_JOBS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<FieldJobDto>>>> GetBySupervisorId([FromRoute] int supervisorId)
    {
        _logger.LogInformation("User {User} fetching field jobs for supervisor: {SupervisorId}", User.Identity?.Name, supervisorId);

        var query = new GetFieldJobsBySupervisorIdQuery(supervisorId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<FieldJobDto>>()
            .SetSuccess(result, "Field jobs for supervisor retrieved successfully"));
    }

    /// <summary>
    /// Create a new field job
    /// </summary>
    /// <param name="command">Field job creation command</param>
    /// <returns>Created field job ID</returns>
    [HttpPost]
    [RequirePermission("FIELD_JOBS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<long>>> Create(
        [FromBody] CreateFieldJobCommand command)
    {
        _logger.LogInformation("User {User} creating field job with JobNumber: {JobNumber}",
            User.Identity?.Name, command?.JobNumber);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<long>()
                .SetSuccess(result, "Field job created successfully"));
    }

    /// <summary>
    /// Update an existing field job
    /// </summary>
    /// <param name="id">Field job ID (bigint)</param>
    /// <param name="command">Field job update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("FIELD_JOBS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] long id,
        [FromBody] UpdateFieldJobCommand command)
    {
        _logger.LogInformation("User {User} updating field job with id: {FieldJobId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Field job with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Field job updated successfully"));
    }

    /// <summary>
    /// Delete a field job
    /// </summary>
    /// <param name="id">Field job ID (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("FIELD_JOBS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting field job with id: {FieldJobId}", User.Identity?.Name, id);

        var command = new DeleteFieldJobCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Field job with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Field job deleted successfully"));
    }
}
