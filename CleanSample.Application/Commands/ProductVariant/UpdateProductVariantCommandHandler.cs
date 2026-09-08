using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.ProductVariant;

public class UpdateProductVariantCommandHandler : IRequestHandler<UpdateProductVariantCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductVariantCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
    {
        var variant = await _unitOfWork.ProductVariants.GetByIdAsync(request.Id);
        if (variant == null)
        {
            return false;
        }

        variant.ProductId = request.ProductId;
        variant.ColorId = request.ColorId;
        variant.MaterialId = request.MaterialId;
        variant.DesignId = request.DesignId;
        variant.Code = request.Code;
        variant.Barcode = request.Barcode;
        variant.Description = request.Description;
        variant.IsActive = request.IsActive;
        variant.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.ProductVariants.UpdateAsync(variant);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
