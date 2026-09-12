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
            CategoryName = p.Category?.Name,
            ColorId = p.ColorId,
            ColorName = p.Color?.Name,
            MaterialId = p.MaterialId,
            MaterialName = p.Material?.Name,
            DesignId = p.DesignId,
            DesignName = p.Design?.Name,
            Name = p.Name,
            Barcode = p.Barcode,
            Description = p.Description,
            PictureUrl = p.PictureUrl,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        });
    }
}
