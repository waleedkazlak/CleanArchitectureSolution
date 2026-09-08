using CleanSample.Application.Commands.ProductVariant;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.ProductVariant;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Product Variant operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductVariantsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductVariantsController> _logger;

    public ProductVariantsController(IMediator mediator, ILogger<ProductVariantsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all product variants with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Product variant search filter object</param>
    /// <returns>Paginated list of product variants</returns>
    [HttpPost("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ProductVariantDto>>>> Search(
        [FromBody] ProductVariantSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching product variants", User.Identity?.Name);

        var query = new GetProductVariantsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ProductVariantDto>>()
            .SetSuccess(result, result.TotalCount, "Product variants retrieved successfully"));
    }

    /// <summary>
    /// Get all product variants with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of product variants</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ProductVariantDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching product variants - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new ProductVariantSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetProductVariantsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ProductVariantDto>>()
            .SetSuccess(result, result.TotalCount, "Product variants retrieved successfully"));
    }

    /// <summary>
    /// Get product variant by id
    /// </summary>
    /// <param name="id">Product variant id</param>
    /// <returns>Product variant details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<ProductVariantDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching product variant with id: {VariantId}", User.Identity?.Name, id);

        var query = new GetProductVariantByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<ProductVariantDto>()
                .SetError(404, $"Product variant with id {id} not found"));
        }

        return Ok(new APIBaseResponse<ProductVariantDto>()
            .SetSuccess(result, "Product variant retrieved successfully"));
    }

    /// <summary>
    /// Create a new product variant (Admin only)
    /// </summary>
    /// <param name="command">Product variant creation command</param>
    /// <returns>Created product variant id</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateProductVariantCommand command)
    {
        _logger.LogInformation("User {User} creating new product variant: {Code}", User.Identity?.Name, command?.Code);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<int>()
                .SetSuccess(result, "Product variant created successfully"));
    }

    /// <summary>
    /// Update an existing product variant (Admin only)
    /// </summary>
    /// <param name="id">Product variant id</param>
    /// <param name="command">Product variant update command</param>
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
        [FromBody] UpdateProductVariantCommand command)
    {
        _logger.LogInformation("User {User} updating product variant with id: {VariantId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Product variant with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Product variant updated successfully"));
    }

    /// <summary>
    /// Delete a product variant (Admin only)
    /// </summary>
    /// <param name="id">Product variant id</param>
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
        _logger.LogInformation("User {User} deleting product variant with id: {VariantId}", User.Identity?.Name, id);

        var command = new DeleteProductVariantCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Product variant with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Product variant deleted successfully"));
    }
}
