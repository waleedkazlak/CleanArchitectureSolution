using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.ProductBOM;

public class DeleteProductBOMCommandHandler : IRequestHandler<DeleteProductBOMCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductBOMCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProductBOMCommand request, CancellationToken cancellationToken)
    {
        var productBom = await _unitOfWork.ProductBOMs.GetByIdAsync(request.Id);
        if (productBom == null)
        {
            return false;
        }

        await _unitOfWork.ProductBOMs.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
