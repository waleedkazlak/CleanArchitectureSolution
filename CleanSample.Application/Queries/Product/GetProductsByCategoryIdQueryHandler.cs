using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Product;

public class GetProductsByCategoryIdQueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, IEnumerable<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProductsByCategoryIdQueryHandler> _logger;

    public GetProductsByCategoryIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductsByCategoryIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProductsByCategoryIdQuery for CategoryId: {CategoryId}", request.CategoryId);

        var products = await _unitOfWork.Products.GetByCategoryIdAsync(request.CategoryId);

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            CategoryId = p.CategoryId,
            CategoryName = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.Category?.NameEn, p.Category?.NameAr),
            ColorId = p.ColorId,
            ColorName = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.Color?.NameEn, p.Color?.NameAr),
            MaterialId = p.MaterialId,
            MaterialName = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.Material?.NameEn, p.Material?.NameAr),
            DesignId = p.DesignId,
            DesignName = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.Design?.NameEn, p.Design?.NameAr),
            Name = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.NameEn, p.NameAr) ?? p.NameEn,
            NameEn = p.NameEn,
            NameAr = p.NameAr,
            Barcode = p.Barcode,
            Description = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.DescriptionEn, p.DescriptionAr),
            DescriptionEn = p.DescriptionEn,
            DescriptionAr = p.DescriptionAr,
            PictureUrl = p.PictureUrl,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });
    }
}
