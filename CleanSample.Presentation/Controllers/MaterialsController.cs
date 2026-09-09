using CleanSample.Application.Commands.Material;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Material;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Material operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MaterialsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<MaterialsController> _logger;

    public MaterialsController(IMediator mediator, ILogger<MaterialsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all materials with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Material search filter object</param>
    /// <returns>Paginated list of materials</returns>
    [HttpPost("search")]
    [RequirePermission("MATERIALS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<MaterialDto>>>> Search(
        [FromBody] MaterialSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching materials", User.Identity?.Name);

        var query = new GetMaterialsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<MaterialDto>>()
            .SetSuccess(result, result.TotalCount, "Materials retrieved successfully"));
    }

    /// <summary>
    /// Get all materials with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of materials</returns>
    [HttpGet]
    [RequirePermission("MATERIALS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<MaterialDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching materials - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new MaterialSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetMaterialsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<MaterialDto>>()
            .SetSuccess(result, result.TotalCount, "Materials retrieved successfully"));
    }

    /// <summary>
    /// Get material by id
    /// </summary>
    /// <param name="id">Material id</param>
    /// <returns>Material details</returns>
    [HttpGet("{id}")]
    [RequirePermission("MATERIALS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<MaterialDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching material with id: {MaterialId}", User.Identity?.Name, id);

        var query = new GetMaterialByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<MaterialDto>()
                .SetError(404, $"Material with id {id} not found"));
        }

        return Ok(new APIBaseResponse<MaterialDto>()
            .SetSuccess(result, "Material retrieved successfully"));
    }

    /// <summary>
    /// Create a new material
    /// </summary>
    /// <param name="command">Material creation command</param>
    /// <returns>Created material id</returns>
    [HttpPost]
    [RequirePermission("MATERIALS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateMaterialCommand command)
    {
        _logger.LogInformation("User {User} creating new material: {MaterialName}", User.Identity?.Name, command?.Name);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<int>()
                .SetSuccess(result, "Material created successfully"));
    }

    /// <summary>
    /// Update an existing material
    /// </summary>
    /// <param name="id">Material id</param>
    /// <param name="command">Material update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("MATERIALS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] int id,
        [FromBody] UpdateMaterialCommand command)
    {
        _logger.LogInformation("User {User} updating material with id: {MaterialId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Material with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Material updated successfully"));
    }

    /// <summary>
    /// Delete a material
    /// </summary>
    /// <param name="id">Material id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("MATERIALS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] int id)
    {
        _logger.LogInformation("User {User} deleting material with id: {MaterialId}", User.Identity?.Name, id);

        var command = new DeleteMaterialCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Material with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Material deleted successfully"));
    }
}
