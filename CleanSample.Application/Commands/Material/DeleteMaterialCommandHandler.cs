using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Material;

public class DeleteMaterialCommandHandler : IRequestHandler<DeleteMaterialCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteMaterialCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _unitOfWork.Materials.GetByIdAsync(request.Id);
        if (material == null)
        {
            return false;
        }

        await _unitOfWork.Materials.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
