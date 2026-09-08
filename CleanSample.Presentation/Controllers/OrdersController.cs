using CleanSample.Application.Commands.Order;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Order;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Order operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all orders with advanced filtering, search, and pagination
    /// </summary>
    /// <param name="filter">Order search filter object</param>
    /// <returns>Paginated list of orders</returns>
    [HttpPost("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<OrderDto>>>> Search(
        [FromBody] OrderSearchFilterDto filter)
    {
        _logger.LogInformation("User {User} searching orders", User.Identity?.Name);

        var query = new GetOrdersWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<OrderDto>>()
            .SetSuccess(result, result.TotalCount, "Orders retrieved successfully"));
    }

    /// <summary>
    /// Get all orders with pagination (simplified)
    /// </summary>
    /// <param name="pageNumber">Page number (1-based), defaults to 1</param>
    /// <param name="pageSize">Number of items per page, defaults to 10</param>
    /// <returns>Paginated list of orders</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<OrderDto>>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("User {User} fetching orders - Page: {PageNumber}", User.Identity?.Name, pageNumber);

        var filter = new OrderSearchFilterDto
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var query = new GetOrdersWithFilterQuery(filter);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<PaginatedResultDto<OrderDto>>()
            .SetSuccess(result, result.TotalCount, "Orders retrieved successfully"));
    }

    /// <summary>
    /// Get order by id
    /// </summary>
    /// <param name="id">Order id (bigint)</param>
    /// <returns>Order details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<OrderDto>>> GetById([FromRoute] long id)
    {
        _logger.LogInformation("User {User} fetching order with id: {OrderId}", User.Identity?.Name, id);

        var query = new GetOrderByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new APIBaseResponse<OrderDto>()
                .SetError(404, $"Order with id {id} not found"));
        }

        return Ok(new APIBaseResponse<OrderDto>()
            .SetSuccess(result, "Order retrieved successfully"));
    }

    /// <summary>
    /// Get all orders for a specific client
    /// </summary>
    /// <param name="clientId">Client id</param>
    /// <returns>List of orders</returns>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<OrderDto>>>> GetByClientId([FromRoute] int clientId)
    {
        _logger.LogInformation("User {User} fetching orders for client: {ClientId}", User.Identity?.Name, clientId);

        var query = new GetOrdersByClientIdQuery(clientId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<OrderDto>>()
            .SetSuccess(result, "Orders for client retrieved successfully"));
    }

    /// <summary>
    /// Create a new order (Admin only)
    /// </summary>
    /// <param name="command">Order creation command</param>
    /// <returns>Created order id (bigint)</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<long>>> Create([FromBody] CreateOrderCommand command)
    {
        _logger.LogInformation("User {User} creating new order: {OrderNumber}", User.Identity?.Name, command?.OrderNumber);

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id = result },
            new APIBaseResponse<long>()
                .SetSuccess(result, "Order created successfully"));
    }

    /// <summary>
    /// Update an existing order (Admin only)
    /// </summary>
    /// <param name="id">Order id (bigint)</param>
    /// <param name="command">Order update command</param>
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
        [FromRoute] long id,
        [FromBody] UpdateOrderCommand command)
    {
        _logger.LogInformation("User {User} updating order with id: {OrderId}", User.Identity?.Name, id);

        if (id != command.Id)
        {
            command.Id = id;
        }

        var result = await _mediator.Send(command);
        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Order with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Order updated successfully"));
    }

    /// <summary>
    /// Delete an order (Admin only)
    /// </summary>
    /// <param name="id">Order id (bigint)</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete([FromRoute] long id)
    {
        _logger.LogInformation("User {User} deleting order with id: {OrderId}", User.Identity?.Name, id);

        var command = new DeleteOrderCommand(id);
        var result = await _mediator.Send(command);

        if (!result)
        {
            return NotFound(new APIBaseResponse<bool>()
                .SetError(404, $"Order with id {id} not found"));
        }

        return Ok(new APIBaseResponse<bool>()
            .SetSuccess(true, "Order deleted successfully"));
    }
}
