using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Color;

public class UpdateColorCommandHandler : IRequestHandler<UpdateColorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateColorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.Colors.GetByIdAsync(request.Id);
        if (color == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.NameEn))
            color.NameEn = request.NameEn;
        else if (!string.IsNullOrWhiteSpace(request.Name))
            color.NameEn = request.Name;

        if (request.NameAr != null)
            color.NameAr = request.NameAr;

        color.Code = request.Code;
        color.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Colors.UpdateAsync(color);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
