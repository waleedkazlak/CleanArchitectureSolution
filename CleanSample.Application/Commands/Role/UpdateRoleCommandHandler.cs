using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Role;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.GetByIdAsync(request.Id);
        if (role == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.NameEn))
            role.NameEn = request.NameEn;
        else if (!string.IsNullOrWhiteSpace(request.Name))
            role.NameEn = request.Name;

        if (request.NameAr != null)
            role.NameAr = request.NameAr;

        if (!string.IsNullOrWhiteSpace(request.DescriptionEn))
            role.DescriptionEn = request.DescriptionEn;
        else if (request.Description != null)
            role.DescriptionEn = request.Description;

        if (request.DescriptionAr != null)
            role.DescriptionAr = request.DescriptionAr;

        role.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Roles.UpdateAsync(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
