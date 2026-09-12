using CleanSample.Application.DTOs;
using CleanSample.Application.Queries.Lookup;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for System Status Lookups
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LookupsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<LookupsController> _logger;

    public LookupsController(IMediator mediator, ILogger<LookupsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all status lookups across the entire system
    /// </summary>
    [HttpGet("all-statuses")]
    [ProducesResponseType(typeof(APIBaseResponse<AllStatusesLookupDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<AllStatusesLookupDto>>> GetAllStatuses()
    {
        _logger.LogInformation("User {User} fetching all status lookups", User.Identity?.Name);
        var result = await _mediator.Send(new GetAllStatusesLookupQuery());
        return Ok(new APIBaseResponse<AllStatusesLookupDto>().SetSuccess(result, "All status lookups retrieved successfully"));
    }

    /// <summary>
    /// Get Order status lookups
    /// </summary>
    [HttpGet("order-statuses")]
    [ProducesResponseType(typeof(APIBaseResponse<List<StatusLookupDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<List<StatusLookupDto>>>> GetOrderStatuses()
    {
        var result = await _mediator.Send(new GetOrderStatusesLookupQuery());
        return Ok(new APIBaseResponse<List<StatusLookupDto>>().SetSuccess(result, "Order statuses retrieved successfully"));
    }

    /// <summary>
    /// Get Load Request status lookups
    /// </summary>
    [HttpGet("load-request-statuses")]
    [ProducesResponseType(typeof(APIBaseResponse<List<StatusLookupDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<List<StatusLookupDto>>>> GetLoadRequestStatuses()
    {
        var result = await _mediator.Send(new GetLoadRequestStatusesLookupQuery());
        return Ok(new APIBaseResponse<List<StatusLookupDto>>().SetSuccess(result, "Load request statuses retrieved successfully"));
    }

    /// <summary>
    /// Get Vehicle Load status lookups
    /// </summary>
    [HttpGet("vehicle-load-statuses")]
    [ProducesResponseType(typeof(APIBaseResponse<List<StatusLookupDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<List<StatusLookupDto>>>> GetVehicleLoadStatuses()
    {
        var result = await _mediator.Send(new GetVehicleLoadStatusesLookupQuery());
        return Ok(new APIBaseResponse<List<StatusLookupDto>>().SetSuccess(result, "Vehicle load statuses retrieved successfully"));
    }

    /// <summary>
    /// Get Vehicle Offload status lookups
    /// </summary>
    [HttpGet("vehicle-offload-statuses")]
    [ProducesResponseType(typeof(APIBaseResponse<List<StatusLookupDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<List<StatusLookupDto>>>> GetVehicleOffloadStatuses()
    {
        var result = await _mediator.Send(new GetVehicleOffloadStatusesLookupQuery());
        return Ok(new APIBaseResponse<List<StatusLookupDto>>().SetSuccess(result, "Vehicle offload statuses retrieved successfully"));
    }

    /// <summary>
    /// Get Field Job status lookups
    /// </summary>
    [HttpGet("field-job-statuses")]
    [ProducesResponseType(typeof(APIBaseResponse<List<StatusLookupDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<List<StatusLookupDto>>>> GetFieldJobStatuses()
    {
        var result = await _mediator.Send(new GetFieldJobStatusesLookupQuery());
        return Ok(new APIBaseResponse<List<StatusLookupDto>>().SetSuccess(result, "Field job statuses retrieved successfully"));
    }

    /// <summary>
    /// Get Field Assembly status lookups
    /// </summary>
    [HttpGet("field-assembly-statuses")]
    [ProducesResponseType(typeof(APIBaseResponse<List<StatusLookupDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<List<StatusLookupDto>>>> GetFieldAssemblyStatuses()
    {
        var result = await _mediator.Send(new GetFieldAssemblyStatusesLookupQuery());
        return Ok(new APIBaseResponse<List<StatusLookupDto>>().SetSuccess(result, "Field assembly statuses retrieved successfully"));
    }

    /// <summary>
    /// Get Issue status lookups
    /// </summary>
    [HttpGet("issue-statuses")]
    [ProducesResponseType(typeof(APIBaseResponse<List<StatusLookupDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<APIBaseResponse<List<StatusLookupDto>>>> GetIssueStatuses()
    {
        var result = await _mediator.Send(new GetIssueStatusesLookupQuery());
        return Ok(new APIBaseResponse<List<StatusLookupDto>>().SetSuccess(result, "Issue statuses retrieved successfully"));
    }
}
