using CleanSample.Application.Services;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.User;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationService authenticationService,
        ILogger<CreateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating user with username: {UserName}", request.UserName);

        var existingUser = await _unitOfWork.Users.GetByUserNameAsync(request.UserName);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"User with username '{request.UserName}' already exists.");
        }

        var rawPassword = string.IsNullOrWhiteSpace(request.Password) ? "P@$$w0rd" : request.Password;
        var (hash, salt) = _authenticationService.HashPasswordWithSalt(rawPassword);

        var fullNameEn = !string.IsNullOrWhiteSpace(request.FullNameEn) ? request.FullNameEn : request.FullName;
        var preferredLanguage = !string.IsNullOrWhiteSpace(request.PreferredLanguage) ? request.PreferredLanguage : "en";

        var user = new Domain.Entities.User
        {
            RoleId = request.RoleId,
            UserName = request.UserName,
            FullNameEn = fullNameEn,
            FullNameAr = request.FullNameAr,
            Email = request.Email,
            Mobile = request.Mobile,
            PasswordHash = hash,
            PasswordSalt = salt,
            PreferredLanguage = preferredLanguage,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        var userId = await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created user with ID: {UserId}", userId);
        return userId;
    }
}

