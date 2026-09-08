using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.ProductVariant;

public class GetProductVariantByIdQueryHandler : IRequestHandler<GetProductVariantByIdQuery, ProductVariantDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductVariantByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductVariantDto?> Handle(GetProductVariantByIdQuery request, CancellationToken cancellationToken)
    {
        var variant = await _unitOfWork.ProductVariants.GetByIdAsync(request.Id);
        if (variant == null)
        {
            return null;
        }

        return new ProductVariantDto
        {
            Id = variant.Id,
            ProductId = variant.ProductId,
            ProductName = variant.Product?.Name,
            ColorId = variant.ColorId,
            ColorName = variant.Color?.Name,
            MaterialId = variant.MaterialId,
            MaterialName = variant.Material?.Name,
            DesignId = variant.DesignId,
            DesignName = variant.Design?.Name,
            Code = variant.Code,
            Barcode = variant.Barcode,
            Description = variant.Description,
            IsActive = variant.IsActive,
            CreatedAt = variant.CreatedAt,
            UpdatedAt = variant.UpdatedAt
        };
    }
}
