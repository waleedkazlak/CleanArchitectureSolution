using CleanSample.Application.DTOs;
using CleanSample.Application.Services;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Product;

public class UploadProductPictureCommandHandler : IRequestHandler<UploadProductPictureCommand, UploadProductPictureResultDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<UploadProductPictureCommandHandler> _logger;

    private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB

    public UploadProductPictureCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<UploadProductPictureCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public async Task<UploadProductPictureResultDto?> Handle(UploadProductPictureCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UploadProductPictureCommand for ProductId: {ProductId}, FileName: {FileName}", 
            request.ProductId, request.FileName);

        if (request.Length > MaxFileSizeInBytes)
        {
            throw new ArgumentException($"File size exceeds the maximum limit of {MaxFileSizeInBytes / (1024 * 1024)} MB.");
        }

        CleanSample.Domain.Entities.Product? product = null;

        if (request.ProductId.HasValue)
        {
            product = await _unitOfWork.Products.GetByIdAsync(request.ProductId.Value);
            if (product == null)
            {
                _logger.LogWarning("Product with Id {ProductId} not found", request.ProductId.Value);
                return null;
            }

            // Clean up previous image file if exists
            if (!string.IsNullOrWhiteSpace(product.PictureUrl))
            {
                await _fileStorageService.DeleteFileAsync(product.PictureUrl, cancellationToken);
            }
        }

        // Save picture file to storage
        var pictureUrl = await _fileStorageService.SaveFileAsync(
            request.FileStream,
            request.FileName,
            "uploads/products",
            cancellationToken);

        ProductDto? productDto = null;

        if (product != null)
        {
            product.PictureUrl = pictureUrl;
            product.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            productDto = new ProductDto
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
        }

        _logger.LogInformation("Successfully saved product picture: {PictureUrl}", pictureUrl);

        return new UploadProductPictureResultDto
        {
            PictureUrl = pictureUrl,
            FileName = request.FileName,
            ProductId = request.ProductId,
            Product = productDto
        };
    }
}
