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
            if (string.IsNullOrWhiteSpace(request.Request.Username) || string.IsNullOrWhiteSpace(request.Request.Password))
            {
                _logger.LogWarning("Empty username or password provided for login attempt");
                throw new InvalidOperationException("Invalid username or password");
            }

            // Get user from database
            var user = await _unitOfWork.Users.GetByUserNameAsync(request.Request.Username.Trim());

            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", request.Request.Username);
                throw new InvalidOperationException("Invalid username or password");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Inactive user attempted to login: {Username}", request.Request.Username);
                throw new InvalidOperationException("User account is disabled. Please contact your administrator.");
            }

            // Verify salted password
            var isPasswordValid = _authenticationService.VerifyPassword(
                request.Request.Password,
                user.PasswordHash,
                user.PasswordSalt);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Invalid password provided for user: {Username}", request.Request.Username);
                throw new InvalidOperationException("Invalid username or password");
            }

            // Load user permissions
            var roleName = user.Role?.Name ?? "User";
            var activeScreens = (await _unitOfWork.Screens.GetActiveAsync(cancellationToken))
                .DistinctBy(s => s.Code)
                .ToList();
            var permissionsList = new List<string>();
            var screenPermissionItems = new List<ScreenPermissionItemDto>();

            if (user.RoleId.HasValue)
            {
                var isAdmin = roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase);
                var rolePermissions = isAdmin
                    ? null
                    : await _unitOfWork.RolePermissions.GetByRoleIdAsync(user.RoleId.Value, cancellationToken);

                var permsDict = rolePermissions?
                    .GroupBy(rp => rp.ScreenId)
                    .ToDictionary(g => g.Key, g => g.First());

                foreach (var screen in activeScreens)
                {
                    if (isAdmin)
                    {
                        permissionsList.Add($"{screen.Code}.VIEW");
                        permissionsList.Add($"{screen.Code}.CREATE");
                        permissionsList.Add($"{screen.Code}.UPDATE");
                        permissionsList.Add($"{screen.Code}.DELETE");

                        screenPermissionItems.Add(new ScreenPermissionItemDto
                        {
                            ScreenId = screen.Id,
                            ScreenName = screen.Name,
                            ScreenCode = screen.Code,
                            Module = screen.Module,
                            Description = screen.Description,
                            CanView = true,
                            CanCreate = true,
                            CanUpdate = true,
                            CanDelete = true
                        });
                    }
                    else if (permsDict != null && permsDict.TryGetValue(screen.Id, out var rp))
                    {
                        if (rp.CanView) permissionsList.Add($"{screen.Code}.VIEW");
                        if (rp.CanCreate) permissionsList.Add($"{screen.Code}.CREATE");
                        if (rp.CanUpdate) permissionsList.Add($"{screen.Code}.UPDATE");
                        if (rp.CanDelete) permissionsList.Add($"{screen.Code}.DELETE");

                        screenPermissionItems.Add(new ScreenPermissionItemDto
                        {
                            ScreenId = screen.Id,
                            ScreenName = screen.Name,
                            ScreenCode = screen.Code,
                            Module = screen.Module,
                            Description = screen.Description,
                            CanView = rp.CanView,
                            CanCreate = rp.CanCreate,
                            CanUpdate = rp.CanUpdate,
                            CanDelete = rp.CanDelete
                        });
                    }
                    else
                    {
                        screenPermissionItems.Add(new ScreenPermissionItemDto
                        {
                            ScreenId = screen.Id,
                            ScreenName = screen.Name,
                            ScreenCode = screen.Code,
                            Module = screen.Module,
                            Description = screen.Description,
                            CanView = false,
                            CanCreate = false,
                            CanUpdate = false,
                            CanDelete = false
                        });
                    }
                }
            }

            var uniquePermissions = permissionsList.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            // Generate JWT token with role and screen permissions
            var token = await _authenticationService.GenerateTokenAsync(
                user.Id,
                user.UserName,
                user.Email ?? string.Empty,
                user.FullName,
                roleName,
                user.RoleId,
                uniquePermissions);

            _logger.LogInformation("Login successful for user: {Username} with role: {Role} and {Count} permissions",
                request.Request.Username, roleName, uniquePermissions.Count);

            return new LoginResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = _jwtSettings.ExpirationMinutes * 60,
                Permissions = uniquePermissions,
                ScreenPermissions = screenPermissionItems,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email ?? string.Empty,
                    FullName = user.FullName,
                    Role = roleName,
                    RoleId = user.RoleId
                }
            };

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during login for username: {Username}", request.Request.Username);
            throw;
        }
    }
}
