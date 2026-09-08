using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.ProductBOM;

public class CreateProductBOMCommandHandler : IRequestHandler<CreateProductBOMCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductBOMCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateProductBOMCommand request, CancellationToken cancellationToken)
    {
        var productBom = new CleanSample.Domain.Entities.ProductBOM
        {
            ProductVariantId = request.ProductVariantId,
            PartId = request.PartId,
            Quantity = request.Quantity
        };

        var productBomId = await _unitOfWork.ProductBOMs.AddAsync(productBom);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return productBomId;
    }
}
