using CleanSample.Application.Commands.ProductBOM;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.ProductBOM;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for ProductBOM operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductBOMController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductBOMController> _logger;

    public ProductBOMController(IMediator mediator, ILogger<ProductBOMController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all product BOMs with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Product BOM search filter object</param>
    /// <returns>Paginated list of product BOMs</returns>
    [HttpPost("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ProductBOMDto>>>> Search(
        [FromBody] ProductBOMSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching product BOMs", User.Identity?.Name);

        var query = new GetProductBOMsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ProductBOMDto>>()
            .SetSuccess(result, result.TotalCount, "Product BOMs retrieved successfully"));
    }

    /// <summary>
    /// Get all product BOMs with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of product BOMs</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ProductBOMDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching product BOMs - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new ProductBOMSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetProductBOMsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ProductBOMDto>>()
            .SetSuccess(result, result.TotalCount, "Product BOMs retrieved successfully"));
    }

    /// <summary>
    /// Get product BOM by id
    /// </summary>
    /// <param name="id">Product BOM id</param>
    /// <returns>Product BOM details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<ProductBOMDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching product BOM with id: {ProductBOMId}", User.Identity?.Name, id);

        var query = new GetProductBOMByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<ProductBOMDto>()
                .SetError(404, $"Product BOM with id {id} not found"));
        }

        return Ok(new APIBaseResponse<ProductBOMDto>()
            .SetSuccess(result, "Product BOM retrieved successfully"));
    }

    /// <summary>
    /// Get all product BOMs for a specific product variant
    /// </summary>
    /// <param name="productVariantId">Product variant id</param>
    /// <returns>List of product BOMs</returns>
    [HttpGet("variant/{productVariantId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<ProductBOMDto>>>> GetByVariantId([FromRoute] int productVariantId)
    {
        _logger.LogInformation("User {User} fetching product BOMs for variant: {ProductVariantId}", User.Identity?.Name, productVariantId);

        var query = new GetProductBOMsByVariantIdQuery(productVariantId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<ProductBOMDto>>()
            .SetSuccess(result, "Product BOMs for variant retrieved successfully"));
    }

    /// <summary>
    /// Create a new product BOM (Admin only)
    /// </summary>
    /// <param name="command">Product BOM creation command</param>
    /// <returns>Created product BOM id</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateProductBOMCommand command)
    {
        _logger.LogInformation("User {User} creating new product BOM for variant: {VariantId}, part: {PartId}",
            User.Identity?.Name, command?.ProductVariantId, command?.PartId);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<int>()
                .SetSuccess(result, "Product BOM created successfully"));
    }

    /// <summary>
    /// Update an existing product BOM (Admin only)
    /// </summary>
    /// <param name="id">Product BOM id</param>
    /// <param name="command">Product BOM update command</param>
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
        [FromBody] UpdateProductBOMCommand command)
    {
        _logger.LogInformation("User {User} updating product BOM with id: {ProductBOMId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Product BOM with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Product BOM updated successfully"));
    }

    /// <summary>
    /// Delete a product BOM (Admin only)
    /// </summary>
    /// <param name="id">Product BOM id</param>
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
        _logger.LogInformation("User {User} deleting product BOM with id: {ProductBOMId}", User.Identity?.Name, id);

        var command = new DeleteProductBOMCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Product BOM with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Product BOM deleted successfully"));
    }
}
