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

        if (!string.IsNullOrWhiteSpace(request.NameEn))
            material.NameEn = request.NameEn;
        else if (!string.IsNullOrWhiteSpace(request.Name))
            material.NameEn = request.Name;

        if (request.NameAr != null)
            material.NameAr = request.NameAr;

        if (!string.IsNullOrWhiteSpace(request.DescriptionEn))
            material.DescriptionEn = request.DescriptionEn;
        else if (request.Description != null)
            material.DescriptionEn = request.Description;

        if (request.DescriptionAr != null)
            material.DescriptionAr = request.DescriptionAr;

        material.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Materials.UpdateAsync(material);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
