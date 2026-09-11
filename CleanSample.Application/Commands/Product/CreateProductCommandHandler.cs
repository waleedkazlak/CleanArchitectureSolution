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
        var product = new CleanSample.Domain.Entities.Product
        {
            CategoryId = request.CategoryId,
            ColorId = request.ColorId,
            MaterialId = request.MaterialId,
            DesignId = request.DesignId,
            Name = request.Name,
            Barcode = request.Barcode,
            Description = request.Description,
            IsActive = request.IsActive
        };

        var productId = await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return productId;
    }
}
