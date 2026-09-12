using CleanSample.Application.Common;
using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.LoadRequestPart;
using CleanSample.Presentation.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for LoadRequestParts operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoadRequestPartsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LoadRequestPartsController> _logger;

    public LoadRequestPartsController(IMediator mediator, ILogger<LoadRequestPartsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get load request parts summary by LoadRequestId (returns LoadRequestId, ProductId, PartId, RequiredQuantity)
    /// </summary>
    /// <param name="loadRequestId">Load request ID (bigint)</param>
    /// <returns>List of parts for the load request</returns>
    [HttpGet("by-load-request/{loadRequestId}")]
    [HttpGet("{loadRequestId}")]
    [RequirePermission("LOAD_REQUESTS", PermissionAction.View)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IEnumerable<LoadRequestPartSummaryDto>>>> GetByLoadRequestId([FromRoute] long loadRequestId)
    {
        _logger.LogInformation("User {User} fetching parts for load request: {LoadRequestId}", User.Identity?.Name, loadRequestId);

        var query = new GetLoadRequestPartsByLoadRequestIdQuery(loadRequestId);
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IEnumerable<LoadRequestPartSummaryDto>>()
            .SetSuccess(result, "Load request parts retrieved successfully"));
    }
}
