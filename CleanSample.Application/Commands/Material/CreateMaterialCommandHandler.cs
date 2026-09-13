using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Material;

public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMaterialCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        var nameEn = !string.IsNullOrWhiteSpace(request.NameEn) ? request.NameEn : (request.Name ?? string.Empty);
        var nameAr = request.NameAr;
        var descEn = !string.IsNullOrWhiteSpace(request.DescriptionEn) ? request.DescriptionEn : request.Description;
        var descAr = request.DescriptionAr;

        var material = new CleanSample.Domain.Entities.Material
        {
            NameEn = nameEn,
            NameAr = nameAr,
            DescriptionEn = descEn,
            DescriptionAr = descAr
        };

        var materialId = await _unitOfWork.Materials.AddAsync(material);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return materialId;
    }
}
