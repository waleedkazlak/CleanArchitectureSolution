using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.Auth;

/// <summary>
/// Command for user login
/// </summary>
public class LoginCommand : IRequest<LoginResponseDto>
{
    /// <summary>
    /// Login request data
    /// </summary>
    public LoginRequestDto Request { get; set; } = new();

    public LoginCommand()
    {
    }

    public LoginCommand(LoginRequestDto request)
    {
        Request = request;
    }
}