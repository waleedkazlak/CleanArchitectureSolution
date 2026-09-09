using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.User;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateUserCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
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

        var user = new Domain.Entities.User
        {
            RoleId = request.RoleId,
            UserName = request.UserName,
            FullName = request.FullName,
            Email = request.Email,
            Mobile = request.Mobile,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        var userId = await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created user with ID: {UserId}", userId);
        return userId;
    }
}
