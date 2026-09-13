using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Product;

/// <summary>
/// Handler for GetProductByIdQuery
/// </summary>
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProductByIdQuery for product id: {ProductId}", request.Id);

        try
        {
            // Use the UnitOfWork to access the repository
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found", request.Id);
                return null;
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                CategoryName = CleanSample.Application.Helpers.LocalizationHelper.Localize(product.Category?.NameEn, product.Category?.NameAr),
                ColorId = product.ColorId,
                ColorName = CleanSample.Application.Helpers.LocalizationHelper.Localize(product.Color?.NameEn, product.Color?.NameAr),
                MaterialId = product.MaterialId,
                MaterialName = CleanSample.Application.Helpers.LocalizationHelper.Localize(product.Material?.NameEn, product.Material?.NameAr),
                DesignId = product.DesignId,
                DesignName = CleanSample.Application.Helpers.LocalizationHelper.Localize(product.Design?.NameEn, product.Design?.NameAr),
                Name = CleanSample.Application.Helpers.LocalizationHelper.Localize(product.NameEn, product.NameAr) ?? product.NameEn,
                NameEn = product.NameEn,
                NameAr = product.NameAr,
                Barcode = product.Barcode,
                Description = CleanSample.Application.Helpers.LocalizationHelper.Localize(product.DescriptionEn, product.DescriptionAr),
                DescriptionEn = product.DescriptionEn,
                DescriptionAr = product.DescriptionAr,
                PictureUrl = product.PictureUrl,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            _logger.LogInformation("Successfully mapped product with id {ProductId} to DTO", request.Id);
            return productDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while handling GetProductByIdQuery for product id: {ProductId}", request.Id);
            throw;
        }
    }
}
