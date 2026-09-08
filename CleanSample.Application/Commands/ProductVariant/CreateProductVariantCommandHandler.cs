using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.ProductVariant;

public class CreateProductVariantCommandHandler : IRequestHandler<CreateProductVariantCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductVariantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var variant = new CleanSample.Domain.Entities.ProductVariant
        {
            ProductId = request.ProductId,
            ColorId = request.ColorId,
            MaterialId = request.MaterialId,
            DesignId = request.DesignId,
            Code = request.Code,
            Barcode = request.Barcode,
            Description = request.Description,
            IsActive = request.IsActive
        };

        var variantId = await _unitOfWork.ProductVariants.AddAsync(variant);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return variantId;
    }
}
