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
        var nameEn = !string.IsNullOrWhiteSpace(request.NameEn) ? request.NameEn : (request.Name ?? string.Empty);
        var nameAr = request.NameAr;
        var descEn = !string.IsNullOrWhiteSpace(request.DescriptionEn) ? request.DescriptionEn : request.Description;
        var descAr = request.DescriptionAr;

        var role = new CleanSample.Domain.Entities.Role
        {
            NameEn = nameEn,
            NameAr = nameAr,
            DescriptionEn = descEn,
            DescriptionAr = descAr
        };

        var roleId = await _unitOfWork.Roles.AddAsync(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return roleId;
    }
}
