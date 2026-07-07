using CleanSample.Application.Commands.Auth;
using CleanSample.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanSample.Presentation.Controllers;

/// <summary>
/// API Controller for authentication operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Login with username and password to get JWT token
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token and user information</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIBaseResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        _logger.LogInformation("Login attempt for username: {Username}", request?.Username);

        try
        {
            var command = new LoginCommand(request);
            var result = await _mediator.Send(command);

            _logger.LogInformation("Login successful for username: {Username}", request?.Username);
            return Ok(new APIBaseResponse<LoginResponseDto>()
                .SetSuccess(result, "Login successful"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Login failed for username: {Username}", request?.Username);
            return Unauthorized(new APIBaseResponse<LoginResponseDto>()
                .SetError(401, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for username: {Username}", request?.Username);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new APIBaseResponse<LoginResponseDto>()
                    .SetError(500, "An unexpected error occurred during login"));
        }
    }
}