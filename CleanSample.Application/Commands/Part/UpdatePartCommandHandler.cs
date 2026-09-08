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
        part.Name = request.Name;
        part.Description = request.Description;
        part.Barcode = request.Barcode;
        part.IsActive = request.IsActive;
        part.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Parts.UpdateAsync(part);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
