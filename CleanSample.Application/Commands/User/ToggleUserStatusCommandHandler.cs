using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.User;

public class ToggleUserStatusCommandHandler : IRequestHandler<ToggleUserStatusCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ToggleUserStatusCommandHandler> _logger;

    public ToggleUserStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<ToggleUserStatusCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Toggling active status for user with ID: {UserId}", request.Id);

        var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
        if (user == null)
        {
            return false;
        }

        user.IsActive = !user.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User {UserId} active status toggled to: {IsActive}", request.Id, user.IsActive);
        return true;
    }
}
