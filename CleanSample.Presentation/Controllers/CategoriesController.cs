using CleanSample.Application.Commands.Category;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Category;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Category operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(IMediator mediator, ILogger<CategoriesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all categories with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Category search filter object</param>
    /// <returns>Paginated list of categories</returns>
    [HttpPost("search")]
    [RequirePermission("CATEGORIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<CategoryDto>>>> Search(
        [FromBody] CategorySearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching categories", User.Identity?.Name);

        var query = new GetCategoriesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<CategoryDto>>()
            .SetSuccess(result, result.TotalCount, "Categories retrieved successfully"));
    }

    /// <summary>
    /// Get all categories with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of categories</returns>
    [HttpGet]
    [RequirePermission("CATEGORIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<CategoryDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching categories - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new CategorySearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetCategoriesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<CategoryDto>>()
            .SetSuccess(result, result.TotalCount, "Categories retrieved successfully"));
    }

    /// <summary>
    /// Get category by id
    /// </summary>
    /// <param name="id">Category id</param>
    /// <returns>Category details</returns>
    [HttpGet("{id}")]
    [RequirePermission("CATEGORIES", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<CategoryDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching category with id: {CategoryId}", User.Identity?.Name, id);

        var query = new GetCategoryByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<CategoryDto>()
                .SetError(404, $"Category with id {id} not found"));
        }

        return Ok(new APIBaseResponse<CategoryDto>()
            .SetSuccess(result, "Category retrieved successfully"));
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="command">Category creation command</param>
    /// <returns>Created category id</returns>
    [HttpPost]
    [RequirePermission("CATEGORIES", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateCategoryCommand command)
    {
        _logger.LogInformation("User {User} creating new category: {CategoryName}", User.Identity?.Name, command?.Name);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<int>()
                .SetSuccess(result, "Category created successfully"));
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    /// <param name="id">Category id</param>
    /// <param name="command">Category update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("CATEGORIES", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] int id,
        [FromBody] UpdateCategoryCommand command)
    {
        _logger.LogInformation("User {User} updating category with id: {CategoryId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Category with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Category updated successfully"));
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    /// <param name="id">Category id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("CATEGORIES", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] int id)
    {
        _logger.LogInformation("User {User} deleting category with id: {CategoryId}", User.Identity?.Name, id);

        var command = new DeleteCategoryCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Category with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Category deleted successfully"));
    }
}
