using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Material;

public class UpdateMaterialCommandHandler : IRequestHandler<UpdateMaterialCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateMaterialCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _unitOfWork.Materials.GetByIdAsync(request.Id);
        if (material == null)
        {
            return false;
        }

        material.Name = request.Name;
        material.Description = request.Description;
        material.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Materials.UpdateAsync(material);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
