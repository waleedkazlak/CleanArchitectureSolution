using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Design;

public class CreateDesignCommandHandler : IRequestHandler<CreateDesignCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDesignCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateDesignCommand request, CancellationToken cancellationToken)
    {
        var nameEn = !string.IsNullOrWhiteSpace(request.NameEn) ? request.NameEn : (request.Name ?? string.Empty);
        var nameAr = request.NameAr;
        var descEn = !string.IsNullOrWhiteSpace(request.DescriptionEn) ? request.DescriptionEn : request.Description;
        var descAr = request.DescriptionAr;

        var design = new CleanSample.Domain.Entities.Design
        {
            NameEn = nameEn,
            NameAr = nameAr,
            DescriptionEn = descEn,
            DescriptionAr = descAr
        };

        var designId = await _unitOfWork.Designs.AddAsync(design);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return designId;
    }
}
