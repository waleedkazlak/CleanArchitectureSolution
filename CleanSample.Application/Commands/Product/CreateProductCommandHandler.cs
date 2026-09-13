using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Product;
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var nameEn = !string.IsNullOrWhiteSpace(request.NameEn) ? request.NameEn : (request.Name ?? string.Empty);
        var nameAr = request.NameAr;
        var descEn = !string.IsNullOrWhiteSpace(request.DescriptionEn) ? request.DescriptionEn : request.Description;
        var descAr = request.DescriptionAr;

        var product = new CleanSample.Domain.Entities.Product
        {
            CategoryId = request.CategoryId,
            ColorId = request.ColorId,
            MaterialId = request.MaterialId,
            DesignId = request.DesignId,
            NameEn = nameEn,
            NameAr = nameAr,
            DescriptionEn = descEn,
            DescriptionAr = descAr,
            Barcode = request.Barcode,
            PictureUrl = request.PictureUrl,
            IsActive = request.IsActive
        };

        var productId = await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return productId;
    }
}
