using CleanSample.Application.Commands.Issue;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Issue;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Issue operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IssuesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<IssuesController> _logger;

    public IssuesController(IMediator mediator, ILogger<IssuesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Search and filter issues with pagination
    /// </summary>
    /// <param name="filter">Issue search filter parameters</param>
    /// <returns>Paginated list of issues</returns>
    [HttpPost("search")]
    [RequirePermission("ISSUES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<IssueDto>>>> Search(
        [FromBody] IssueSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching issues", User.Identity?.Name);

        var query = new GetIssuesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<IssueDto>>()
            .SetSuccess(result, result.TotalCount, "Issues retrieved successfully"));
    }

    /// <summary>
    /// Get all issues with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of issues</returns>
    [HttpGet]
    [RequirePermission("ISSUES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<IssueDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching issues - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new IssueSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetIssuesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<IssueDto>>()
            .SetSuccess(result, result.TotalCount, "Issues retrieved successfully"));
    }

    /// <summary>
    /// Get an issue by its unique ID
    /// </summary>
    /// <param name="id">Issue ID (bigint)</param>
    /// <returns>Issue details</returns>
    [HttpGet("{id}")]
    [RequirePermission("ISSUES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IssueDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching issue with id: {IssueId}", User.Identity?.Name, id);

        var query = new GetIssueByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<IssueDto>()
                .SetError(404, $"Issue with id {id} not found"));
        }

        return Ok(new APIBaseResponse<IssueDto>()
            .SetSuccess(result, "Issue retrieved successfully"));
    }

    /// <summary>
    /// Get all issues for a specific load request
    /// </summary>
    /// <param name="loadRequestId">Load request ID (bigint)</param>
    /// <returns>List of issues</returns>
    [HttpGet("by-load-request/{loadRequestId}")]
    [RequirePermission("ISSUES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<IssueDto>>>> GetByLoadRequestId([FromRoute] long loadRequestId)
    {
        _logger.LogInformation("User {User} fetching issues for load request: {LoadRequestId}", User.Identity?.Name, loadRequestId);

        var query = new GetIssuesByLoadRequestIdQuery(loadRequestId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<IssueDto>>()
            .SetSuccess(result, "Issues for load request retrieved successfully"));
    }

    /// <summary>
    /// Get all issues for a specific field job
    /// </summary>
    /// <param name="fieldJobId">Field job ID (bigint)</param>
    /// <returns>List of issues</returns>
    [HttpGet("by-field-job/{fieldJobId}")]
    [RequirePermission("ISSUES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<IssueDto>>>> GetByFieldJobId([FromRoute] long fieldJobId)
    {
        _logger.LogInformation("User {User} fetching issues for field job: {FieldJobId}", User.Identity?.Name, fieldJobId);

        var query = new GetIssuesByFieldJobIdQuery(fieldJobId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<IssueDto>>()
            .SetSuccess(result, "Issues for field job retrieved successfully"));
    }

    /// <summary>
    /// Get all issues for a specific field assembly
    /// </summary>
    /// <param name="fieldAssemblyId">Field assembly ID (bigint)</param>
    /// <returns>List of issues</returns>
    [HttpGet("by-field-assembly/{fieldAssemblyId}")]
    [RequirePermission("ISSUES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<IssueDto>>>> GetByFieldAssemblyId([FromRoute] long fieldAssemblyId)
    {
        _logger.LogInformation("User {User} fetching issues for field assembly: {FieldAssemblyId}", User.Identity?.Name, fieldAssemblyId);

        var query = new GetIssuesByFieldAssemblyIdQuery(fieldAssemblyId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<IssueDto>>()
            .SetSuccess(result, "Issues for field assembly retrieved successfully"));
    }

    /// <summary>
    /// Get all issues reported by a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of issues</returns>
    [HttpGet("by-reported-by/{userId}")]
    [RequirePermission("ISSUES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<IssueDto>>>> GetByReportedBy([FromRoute] int userId)
    {
        _logger.LogInformation("User {User} fetching issues reported by: {UserId}", User.Identity?.Name, userId);

        var query = new GetIssuesByReportedByQuery(userId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<IssueDto>>()
            .SetSuccess(result, "Issues reported by user retrieved successfully"));
    }

    /// <summary>
    /// Get all issues resolved by a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>List of issues</returns>
    [HttpGet("by-resolved-by/{userId}")]
    [RequirePermission("ISSUES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<IssueDto>>>> GetByResolvedBy([FromRoute] int userId)
    {
        _logger.LogInformation("User {User} fetching issues resolved by: {UserId}", User.Identity?.Name, userId);

        var query = new GetIssuesByResolvedByQuery(userId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<IssueDto>>()
            .SetSuccess(result, "Issues resolved by user retrieved successfully"));
    }

    /// <summary>
    /// Create a new issue
    /// </summary>
    /// <param name="command">Issue creation command</param>
    /// <returns>Created issue ID</returns>
    [HttpPost]
    [RequirePermission("ISSUES", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<long>>> Create(
        [FromBody] CreateIssueCommand command)
    {
        _logger.LogInformation("User {User} creating issue of type: {IssueType}",
            User.Identity?.Name, command?.IssueType);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<long>()
                .SetSuccess(result, "Issue created successfully"));
    }

    /// <summary>
    /// Update an existing issue
    /// </summary>
    /// <param name="id">Issue ID (bigint)</param>
    /// <param name="command">Issue update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("ISSUES", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] long id,
        [FromBody] UpdateIssueCommand command)
    {
        _logger.LogInformation("User {User} updating issue with id: {IssueId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Issue with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Issue updated successfully"));
    }

    /// <summary>
    /// Delete an issue
    /// </summary>
    /// <param name="id">Issue ID (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("ISSUES", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting issue with id: {IssueId}", User.Identity?.Name, id);

        var command = new DeleteIssueCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Issue with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Issue deleted successfully"));
    }
}
