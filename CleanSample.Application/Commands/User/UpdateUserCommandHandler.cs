using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.User;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateUserCommandHandler> _logger;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
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
        user.FullName = request.FullName;
        user.Email = request.Email;
        user.Mobile = request.Mobile;
        user.IsActive = request.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated user with ID: {UserId}", request.Id);
        return true;
    }
}
