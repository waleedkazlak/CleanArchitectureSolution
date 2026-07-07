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
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
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
