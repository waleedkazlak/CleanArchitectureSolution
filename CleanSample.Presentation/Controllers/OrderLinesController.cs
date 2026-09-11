using CleanSample.Application.Commands.OrderLine;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.OrderLine;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for OrderLine operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderLinesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderLinesController> _logger;

    public OrderLinesController(IMediator mediator, ILogger<OrderLinesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all order lines with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Order line search filter object</param>
    /// <returns>Paginated list of order lines</returns>
    [HttpPost("search")]
    [RequirePermission("ORDERS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<OrderLineDto>>>> Search(
        [FromBody] OrderLineSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching order lines", User.Identity?.Name);

        var query = new GetOrderLinesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<OrderLineDto>>()
            .SetSuccess(result, result.TotalCount, "Order lines retrieved successfully"));
    }

    /// <summary>
    /// Get all order lines with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of order lines</returns>
    [HttpGet]
    [RequirePermission("ORDERS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<OrderLineDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching order lines - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new OrderLineSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetOrderLinesWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<OrderLineDto>>()
            .SetSuccess(result, result.TotalCount, "Order lines retrieved successfully"));
    }

    /// <summary>
    /// Get order line by id
    /// </summary>
    /// <param name="id">Order line id (bigint)</param>
    /// <returns>Order line details</returns>
    [HttpGet("{id}")]
    [RequirePermission("ORDERS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<OrderLineDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching order line with id: {OrderLineId}", User.Identity?.Name, id);

        var query = new GetOrderLineByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<OrderLineDto>()
                .SetError(404, $"Order line with id {id} not found"));
        }

        return Ok(new APIBaseResponse<OrderLineDto>()
            .SetSuccess(result, "Order line retrieved successfully"));
    }

    /// <summary>
    /// Get all order lines for a specific order
    /// </summary>
    /// <param name="orderId">Order id (bigint)</param>
    /// <returns>List of order lines</returns>
    [HttpGet("order/{orderId}")]
    [RequirePermission("ORDERS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<OrderLineDto>>>> GetByOrderId([FromRoute] long orderId)
    {
        _logger.LogInformation("User {User} fetching lines for order: {OrderId}", User.Identity?.Name, orderId);

        var query = new GetOrderLinesByOrderIdQuery(orderId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<OrderLineDto>>()
            .SetSuccess(result, "Order lines retrieved successfully"));
    }

    /// <summary>
    /// Create or update order line records (supports single item or list of items)
    /// </summary>
    /// <param name="command">Order line creation/update command</param>
    /// <returns>List of created/updated order lines</returns>
    [HttpPost]
    [RequirePermission("ORDERS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<List<OrderLineDto>>>> Create([FromBody] CreateOrderLineCommand command)
    {
        _logger.LogInformation("User {User} creating/updating order lines", User.Identity?.Name);

        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<List<OrderLineDto>>()
            .SetSuccess(result, result.Count, "Order lines processed successfully"));
    }

    /// <summary>
    /// Batch create or update order lines from a list of items
    /// </summary>
    /// <param name="items">List of order line items to create or update</param>
    /// <returns>List of created/updated order lines</returns>
    [HttpPost("batch")]
    [RequirePermission("ORDERS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<APIBaseResponse<List<OrderLineDto>>>> Batch([FromBody] List<CreateOrderLineItemDto> items)
    {
        _logger.LogInformation("User {User} batch creating/updating {Count} order lines", User.Identity?.Name, items?.Count ?? 0);

        var command = new CreateOrderLineCommand(items ?? new List<CreateOrderLineItemDto>());
        var result = await _mediator.Send(command);

        return Ok(new APIBaseResponse<List<OrderLineDto>>()
            .SetSuccess(result, result.Count, "Order lines processed successfully"));
    }

    /// <summary>
    /// Update an existing order line
    /// </summary>
    /// <param name="id">Order line id (bigint)</param>
    /// <param name="command">Order line update command</param>
    /// <returns>Success status</returns>
    [HttpPut("{id}")]
    [RequirePermission("ORDERS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(
        [FromRoute] long id,
        [FromBody] UpdateOrderLineCommand command)
    {
        _logger.LogInformation("User {User} updating order line with id: {OrderLineId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Order line with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Order line updated successfully"));
    }

    /// <summary>
    /// Delete an order line
    /// </summary>
    /// <param name="id">Order line id (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [RequirePermission("ORDERS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting order line with id: {OrderLineId}", User.Identity?.Name, id);

        var command = new DeleteOrderLineCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Order line with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Order line deleted successfully"));
    }
}
