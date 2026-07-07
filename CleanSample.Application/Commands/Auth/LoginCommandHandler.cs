using CleanSample.Application.Commands.Auth;
using CleanSample.Application.DTOs;
using CleanSample.Application.Services;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands;

/// <summary>
/// Handler for LoginCommand
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthenticationService _authenticationService;
    private readonly JwtSettingsDto _jwtSettings;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationService authenticationService,
        JwtSettingsDto jwtSettings,
        ILogger<LoginCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
        _jwtSettings = jwtSettings;
        _logger = logger;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling login request for username: {Username}", request.Request.Username);

        try
        {
            // Get user from database (for now, we'll use hardcoded demo user)
            // In production, you would query the user repository
            var user = await GetUserAsync(request.Request.Username);

            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", request.Request.Username);
                throw new InvalidOperationException("Invalid username or password");
            }

            // Verify password
            if (!_authenticationService.VerifyPassword(request.Request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid password for user: {Username}", request.Request.Username);
                throw new InvalidOperationException("Invalid username or password");
            }

            // Generate JWT token
            var token = await _authenticationService.GenerateTokenAsync(
                user.Id,
                user.Username,
                user.Email,
                user.FullName,
                user.Role);

            _logger.LogInformation("Login successful for user: {Username}", request.Request.Username);

            return new LoginResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = _jwtSettings.ExpirationMinutes * 60,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for username: {Username}", request.Request.Username);
            throw;
        }
    }

    private async Task<Domain.Entities.User?> GetUserAsync(string username)
    {
        // Demo hardcoded users - replace with repository call in production
        if (username == "admin" && await Task.FromResult(true))
        {
            var passwordHash = _authenticationService.HashPassword("admin123");
            return new Domain.Entities.User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@example.com",
                FullName = "Administrator",
                PasswordHash = passwordHash,
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        if (username == "user" && await Task.FromResult(true))
        {
            var passwordHash = _authenticationService.HashPassword("user123");
            return new Domain.Entities.User
            {
                Id = 2,
                Username = "user",
                Email = "user@example.com",
                FullName = "Regular User",
                PasswordHash = passwordHash,
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        return null;
    }
}