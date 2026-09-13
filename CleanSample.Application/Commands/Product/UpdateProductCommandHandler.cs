using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Product;
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        if (product == null)
            return false;

        product.CategoryId = request.CategoryId;
        product.ColorId = request.ColorId;
        product.MaterialId = request.MaterialId;
        product.DesignId = request.DesignId;
        if (!string.IsNullOrWhiteSpace(request.NameEn))
            product.NameEn = request.NameEn;
        else if (!string.IsNullOrWhiteSpace(request.Name))
            product.NameEn = request.Name;

        if (request.NameAr != null)
            product.NameAr = request.NameAr;

        if (!string.IsNullOrWhiteSpace(request.DescriptionEn))
            product.DescriptionEn = request.DescriptionEn;
        else if (request.Description != null)
            product.DescriptionEn = request.Description;

        if (request.DescriptionAr != null)
            product.DescriptionAr = request.DescriptionAr;

        product.Barcode = request.Barcode;
        product.PictureUrl = request.PictureUrl;
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
