using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Material;

public class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, MaterialDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMaterialByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MaterialDto?> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
    {
        var material = await _unitOfWork.Materials.GetByIdAsync(request.Id);
        if (material == null)
        {
            return null;
        }

        return new MaterialDto
        {
            Id = material.Id,
            Name = CleanSample.Application.Helpers.LocalizationHelper.Localize(material.NameEn, material.NameAr) ?? material.NameEn,
            NameEn = material.NameEn,
            NameAr = material.NameAr,
            Description = CleanSample.Application.Helpers.LocalizationHelper.Localize(material.DescriptionEn, material.DescriptionAr),
            DescriptionEn = material.DescriptionEn,
            DescriptionAr = material.DescriptionAr,
            CreatedAt = material.CreatedAt,
            UpdatedAt = material.UpdatedAt
        };
    }
}
