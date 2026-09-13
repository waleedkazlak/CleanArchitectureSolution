using CleanSample.Application.Services;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.User;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IAuthenticationService authenticationService,
        ILogger<UpdateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", request.Id);

        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
        if (user == null)
        {
            return false;
        }

        user.RoleId = request.RoleId;
        user.UserName = request.UserName;
        if (!string.IsNullOrWhiteSpace(request.FullNameEn))
            user.FullNameEn = request.FullNameEn;
        else if (!string.IsNullOrWhiteSpace(request.FullName))
            user.FullNameEn = request.FullName;

        if (request.FullNameAr != null)
            user.FullNameAr = request.FullNameAr;

        if (!string.IsNullOrWhiteSpace(request.PreferredLanguage))
            user.PreferredLanguage = request.PreferredLanguage;

        user.Email = request.Email;
        user.Mobile = request.Mobile;
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var (hash, salt) = _authenticationService.HashPasswordWithSalt(request.Password);
            user.PasswordHash = hash;
            user.PasswordSalt = salt;
        }

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated user with ID: {UserId}", request.Id);
        return true;
    }
}

public class UpdateUserPreferredLanguageCommandHandler : IRequestHandler<UpdateUserPreferredLanguageCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateUserPreferredLanguageCommandHandler> _logger;

    public UpdateUserPreferredLanguageCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateUserPreferredLanguageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateUserPreferredLanguageCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating preferred language for user ID: {UserId} to {Language}", request.Id, request.PreferredLanguage);

        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
        if (user == null)
        {
            return false;
        }

        user.PreferredLanguage = !string.IsNullOrWhiteSpace(request.PreferredLanguage) ? request.PreferredLanguage.Trim().ToLower() : "en";
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated preferred language for user ID: {UserId}", request.Id);
        return true;
    }
}

