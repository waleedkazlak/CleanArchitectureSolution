using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.ProductBOM;

public class UpdateProductBOMCommandHandler : IRequestHandler<UpdateProductBOMCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductBOMCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateProductBOMCommand request, CancellationToken cancellationToken)
    {
        var productBom = await _unitOfWork.ProductBOMs.GetByIdAsync(request.Id);
        if (productBom == null)
        {
            return false;
        }

        productBom.ProductVariantId = request.ProductVariantId;
        productBom.PartId = request.PartId;
        productBom.Quantity = request.Quantity;
        productBom.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.ProductBOMs.UpdateAsync(productBom);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
