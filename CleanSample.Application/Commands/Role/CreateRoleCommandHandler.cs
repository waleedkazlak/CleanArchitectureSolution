using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Role;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new CleanSample.Domain.Entities.Role
        {
            Name = request.Name
        };

        var roleId = await _unitOfWork.Roles.AddAsync(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return roleId;
    }
}
