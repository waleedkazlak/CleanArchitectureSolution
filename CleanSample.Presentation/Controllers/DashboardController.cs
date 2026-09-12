using CleanSample.Application.DTOs;
using CleanSample.Application.DTOs.Dashboard;
using CleanSample.Application.Queries.Dashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for Dashboard metrics and summaries
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IMediator mediator, ILogger<DashboardController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get comprehensive dashboard metrics covering Orders, Load Requests, Issues, Vehicle Operations, and Field Operations
    /// </summary>
    /// <returns>Consolidated dashboard metrics</returns>
    [HttpGet("metrics")]
    [HttpGet]
    [ProducesResponseType(typeof(APIBaseResponse<DashboardMetricsDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<DashboardMetricsDto>>> GetMetrics()
    {
        _logger.LogInformation("User {User} fetching comprehensive dashboard metrics", User.Identity?.Name);

        var query = new GetDashboardMetricsQuery();
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<DashboardMetricsDto>()
            .SetSuccess(result, "Dashboard metrics retrieved successfully"));
    }

    /// <summary>
    /// Get orders dashboard summary
    /// </summary>
    /// <returns>Orders dashboard summary with status counts</returns>
    [HttpGet("orders")]
    [ProducesResponseType(typeof(APIBaseResponse<OrdersDashboardSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<OrdersDashboardSummaryDto>>> GetOrdersSummary()
    {
        _logger.LogInformation("User {User} fetching orders dashboard summary", User.Identity?.Name);

        var query = new GetOrdersDashboardSummaryQuery();
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<OrdersDashboardSummaryDto>()
            .SetSuccess(result, "Orders dashboard summary retrieved successfully"));
    }

    /// <summary>
    /// Get load requests dashboard summary
    /// </summary>
    /// <returns>Load requests dashboard summary with status breakdown and verification counts</returns>
    [HttpGet("load-requests")]
    [ProducesResponseType(typeof(APIBaseResponse<LoadRequestsDashboardSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<LoadRequestsDashboardSummaryDto>>> GetLoadRequestsSummary()
    {
        _logger.LogInformation("User {User} fetching load requests dashboard summary", User.Identity?.Name);

        var query = new GetLoadRequestsDashboardSummaryQuery();
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<LoadRequestsDashboardSummaryDto>()
            .SetSuccess(result, "Load requests dashboard summary retrieved successfully"));
    }

    /// <summary>
    /// Get issues dashboard summary
    /// </summary>
    /// <returns>Issues dashboard summary with status and severity counts</returns>
    [HttpGet("issues")]
    [ProducesResponseType(typeof(APIBaseResponse<IssuesDashboardSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<IssuesDashboardSummaryDto>>> GetIssuesSummary()
    {
        _logger.LogInformation("User {User} fetching issues dashboard summary", User.Identity?.Name);

        var query = new GetIssuesDashboardSummaryQuery();
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<IssuesDashboardSummaryDto>()
            .SetSuccess(result, "Issues dashboard summary retrieved successfully"));
    }

    /// <summary>
    /// Get vehicle operations dashboard summary (loads, offloads, loaded quantities, verification)
    /// </summary>
    /// <returns>Vehicle operations summary</returns>
    [HttpGet("vehicle-operations")]
    [ProducesResponseType(typeof(APIBaseResponse<VehicleOperationsDashboardSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<VehicleOperationsDashboardSummaryDto>>> GetVehicleOperationsSummary()
    {
        _logger.LogInformation("User {User} fetching vehicle operations dashboard summary", User.Identity?.Name);

        var query = new GetVehicleOperationsDashboardSummaryQuery();
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<VehicleOperationsDashboardSummaryDto>()
            .SetSuccess(result, "Vehicle operations dashboard summary retrieved successfully"));
    }

    /// <summary>
    /// Get field operations dashboard summary (jobs, assemblies, assembled quantities, verification)
    /// </summary>
    /// <returns>Field operations summary</returns>
    [HttpGet("field-operations")]
    [ProducesResponseType(typeof(APIBaseResponse<FieldOperationsDashboardSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<FieldOperationsDashboardSummaryDto>>> GetFieldOperationsSummary()
    {
        _logger.LogInformation("User {User} fetching field operations dashboard summary", User.Identity?.Name);

        var query = new GetFieldOperationsDashboardSummaryQuery();
        var result = await _mediator.Send(query);

        return Ok(new APIBaseResponse<FieldOperationsDashboardSummaryDto>()
            .SetSuccess(result, "Field operations dashboard summary retrieved successfully"));
    }
}
