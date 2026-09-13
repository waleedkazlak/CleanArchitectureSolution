using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Part;

public class UpdatePartCommandHandler : IRequestHandler<UpdatePartCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePartCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdatePartCommand request, CancellationToken cancellationToken)
    {
        var part = await _unitOfWork.Parts.GetByIdAsync(request.Id);
        if (part == null)
        {
            return false;
        }

        part.Code = request.Code;

        if (!string.IsNullOrWhiteSpace(request.NameEn))
            part.NameEn = request.NameEn;
        else if (!string.IsNullOrWhiteSpace(request.Name))
            part.NameEn = request.Name;

        if (request.NameAr != null)
            part.NameAr = request.NameAr;

        if (!string.IsNullOrWhiteSpace(request.DescriptionEn))
            part.DescriptionEn = request.DescriptionEn;
        else if (request.Description != null)
            part.DescriptionEn = request.Description;

        if (request.DescriptionAr != null)
            part.DescriptionAr = request.DescriptionAr;

        part.Barcode = request.Barcode;
        part.IsActive = request.IsActive;
        part.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Parts.UpdateAsync(part);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
