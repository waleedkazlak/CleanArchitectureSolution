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
        product.Name = request.Name;
        product.Barcode = request.Barcode;
        product.Description = request.Description;
        product.PictureUrl = request.PictureUrl;
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
