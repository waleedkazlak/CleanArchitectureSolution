using CleanSample.Application.Commands.Screen;
using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Screen;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScreensController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ScreensController> _logger;

    public ScreensController(IMediator mediator, ILogger<ScreensController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Search screens with advanced filtering and pagination
    /// </summary>
    [HttpPost("search")]
    [RequirePermission("SCREENS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<PaginatedResultDto<ScreenDto>>>> Search([FromBody] ScreenSearchFilterDto filter)
    {
        var result = await _mediator.Send(new GetScreensWithFilterQuery(filter));
        return Ok(new APIBaseResponse<PaginatedResultDto<ScreenDto>>().SetSuccess(result, "Screens retrieved successfully"));
    }

    /// <summary>
    /// Get all screens
    /// </summary>
    [HttpGet]
    [RequirePermission("SCREENS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<IReadOnlyList<ScreenDto>>>> GetAll([FromQuery] bool onlyActive = false)
    {
        var result = await _mediator.Send(new GetAllScreensQuery(onlyActive));
        return Ok(new APIBaseResponse<IReadOnlyList<ScreenDto>>().SetSuccess(result, "Screens retrieved successfully"));
    }

    /// <summary>
    /// Get screen by ID
    /// </summary>
    [HttpGet("{id}")]
    [RequirePermission("SCREENS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIBaseResponse<ScreenDto>>> GetById(int id)
    {
        var result = await _mediator.Send(new GetScreenByIdQuery(id));
        if (result == null)
        {
            return NotFound(new APIBaseResponse<ScreenDto>().SetError(StatusCodes.Status404NotFound, $"Screen with ID {id} not found"));
        }

        return Ok(new APIBaseResponse<ScreenDto>().SetSuccess(result, "Screen retrieved successfully"));
    }

    /// <summary>
    /// Create new screen
    /// </summary>
    [HttpPost]
    [RequirePermission("SCREENS", PermissionAction.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIBaseResponse<int>>> Create([FromBody] CreateScreenCommand command)
    {
        var screenId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = screenId },
            new APIBaseResponse<int>().SetSuccess(screenId, StatusCodes.Status201Created, "Screen created successfully"));
    }

    /// <summary>
    /// Update existing screen
    /// </summary>
    [HttpPut("{id}")]
    [RequirePermission("SCREENS", PermissionAction.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Update(int id, [FromBody] UpdateScreenCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(new APIBaseResponse<bool>().SetError(StatusCodes.Status400BadRequest, "ID mismatch"));
        }

        var result = await _mediator.Send(command);
        return Ok(new APIBaseResponse<bool>().SetSuccess(result, "Screen updated successfully"));
    }

    /// <summary>
    /// Delete screen
    /// </summary>
    [HttpDelete("{id}")]
    [RequirePermission("SCREENS", PermissionAction.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIBaseResponse<bool>>> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteScreenCommand(id));
        return Ok(new APIBaseResponse<bool>().SetSuccess(result, "Screen deleted successfully"));
    }
}
