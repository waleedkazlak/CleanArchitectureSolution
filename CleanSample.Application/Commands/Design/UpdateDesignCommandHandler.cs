using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Design;

public class UpdateDesignCommandHandler : IRequestHandler<UpdateDesignCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDesignCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateDesignCommand request, CancellationToken cancellationToken)
    {
        var design = await _unitOfWork.Designs.GetByIdAsync(request.Id);
        if (design == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.NameEn))
            design.NameEn = request.NameEn;
        else if (!string.IsNullOrWhiteSpace(request.Name))
            design.NameEn = request.Name;

        if (request.NameAr != null)
            design.NameAr = request.NameAr;

        if (!string.IsNullOrWhiteSpace(request.DescriptionEn))
            design.DescriptionEn = request.DescriptionEn;
        else if (request.Description != null)
            design.DescriptionEn = request.Description;

        if (request.DescriptionAr != null)
            design.DescriptionAr = request.DescriptionAr;

        design.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Designs.UpdateAsync(design);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
