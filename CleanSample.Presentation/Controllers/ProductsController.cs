using CleanSample.Application.Commands.Product;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Product;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Product operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all products with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Product search filter object</param>
    /// <returns>Paginated list of products</returns>
    [HttpPost("search")]
    [RequirePermission("PRODUCTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ProductDto>>>> Search(
        [FromBody] ProductSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching products", User.Identity?.Name);
        
        var query = new GetProductsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ProductDto>>()
            .SetSuccess(result, result.TotalCount, "Products retrieved successfully"));
    }

    /// <summary>
    /// Get all products with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of products</returns>
    [HttpGet]
    [RequirePermission("PRODUCTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ProductDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching products - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new ProductSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetProductsWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<ProductDto>>()
            .SetSuccess(result, result.TotalCount, "Products retrieved successfully"));
    }

    /// <summary>
    /// Get product by id
    /// </summary>
    /// <param name="id">Product id</param>
    /// <returns>Product details</returns>
    [HttpGet("{id}")]
    [RequirePermission("PRODUCTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<ProductDto>>> GetById([FromRoute] int id)
    {
        _logger.LogInformation("User {User} fetching product with id: {ProductId}", User.Identity?.Name, id);

        var query = new GetProductByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<ProductDto>()
                .SetError(404, $"Product with id {id} not found"));
        }

        return Ok(new APIBaseResponse<ProductDto>()
            .SetSuccess(result, "Product retrieved successfully"));
    }

    /// <summary>
    /// Get products by category id
    /// </summary>
    /// <param name="categoryId">Category id</param>
    /// <returns>List of products</returns>
    [HttpGet("by-category/{categoryId}")]
    [RequirePermission("PRODUCTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<ProductDto>>>> GetByCategoryId([FromRoute] int categoryId)
    {
        _logger.LogInformation("User {User} fetching products for category: {CategoryId}", User.Identity?.Name, categoryId);

        var query = new GetProductsByCategoryIdQuery(categoryId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<ProductDto>>()
            .SetSuccess(result, "Products for category retrieved successfully"));
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="command">Product creation command</param>
    /// <returns>Created product id</returns>
    [HttpPost]
    [RequirePermission("PRODUCTS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateProductCommand command)
    {
        _logger.LogInformation("User {User} creating new product: {ProductName}", User.Identity?.Name, command?.Name);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<int>()
                .SetSuccess(result, "Product created successfully"));
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <param name="id">Product id</param>
    /// <param name="command">Product update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("PRODUCTS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] int id,
        [FromBody] UpdateProductCommand command)
    {
        _logger.LogInformation("User {User} updating product with id: {ProductId}", User.Identity?.Name, id);

        command.Id = id;
        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Product with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Product updated successfully"));
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="id">Product id</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("PRODUCTS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] int id)
    {
        _logger.LogInformation("User {User} deleting product with id: {ProductId}", User.Identity?.Name, id);

        var command = new DeleteProductCommand { Id = id };
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Product with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Product deleted successfully"));
    }

    /// <summary>
    /// Upload product picture
    /// </summary>
    /// <param name="file">Product picture image file (.jpg, .jpeg, .png, .webp, .gif - max 5MB)</param>
    /// <param name="productId">Optional product id to associate and update directly</param>
    /// <returns>Uploaded picture URL and details</returns>
    [HttpPost("upload-picture")]
    [RequirePermission("PRODUCTS", PermissionAction.Update)]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<UploadProductPictureResultDto>>> UploadPicture(
        IFormFile file,
        [FromForm] int? productId = null)
    {
        _logger.LogInformation("User {User} uploading product picture. ProductId: {ProductId}", User.Identity?.Name, productId);

        if (file == null || file.Length == 0)
        {
            return BadRequest(new APIBaseResponse<UploadProductPictureResultDto>()
                .SetError(400, "Please provide a valid image file."));
        }

        try
        {
            using var stream = file.OpenReadStream();
            var command = new UploadProductPictureCommand
            {
                ProductId = productId,
                FileStream = stream,
                FileName = file.FileName,
                ContentType = file.ContentType,
                Length = file.Length
            };

            var result = await _mediator.Send(command);
            if (result == null && productId.HasValue)
            {
                return NotFound(new APIBaseResponse<UploadProductPictureResultDto>()
                    .SetError(404, $"Product with id {productId} not found"));
            }

            return Ok(new APIBaseResponse<UploadProductPictureResultDto>()
                .SetSuccess(result!, "Product picture uploaded successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new APIBaseResponse<UploadProductPictureResultDto>()
                .SetError(400, ex.Message));
        }
    }
}
