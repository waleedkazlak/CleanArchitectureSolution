using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Product;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PaginatedResultDto<ProductDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllProductsQueryHandler> _logger;

    public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllProductsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling GetAllProductsQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            request.PageNumber, request.PageSize, request.SearchTerm);

        var pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
        var pageSize = request.PageSize > 0 && request.PageSize <= 100 ? request.PageSize : 10;

        var products = await _unitOfWork.Products.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTermLower = request.SearchTerm.ToLower();
            products = products.Where(p =>
                p.Name.ToLower().Contains(searchTermLower) ||
                (p.Barcode != null && p.Barcode.ToLower().Contains(searchTermLower)) ||
                (p.Description != null && p.Description.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        if (request.CategoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == request.CategoryId.Value).ToList();
        }

        if (request.ColorId.HasValue)
        {
            products = products.Where(p => p.ColorId == request.ColorId.Value).ToList();
        }

        if (request.MaterialId.HasValue)
        {
            products = products.Where(p => p.MaterialId == request.MaterialId.Value).ToList();
        }

        if (request.DesignId.HasValue)
        {
            products = products.Where(p => p.DesignId == request.DesignId.Value).ToList();
        }

        if (request.IsActive.HasValue)
        {
            products = products.Where(p => p.IsActive == request.IsActive.Value).ToList();
        }

        products = ApplySort(products.ToList(), request.SortBy, request.SortDirection);

        var totalCount = products.Count();

        var paginatedProducts = products
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var productDtos = paginatedProducts.Select(p => new ProductDto
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
        }).ToList();

        return new PaginatedResultDto<ProductDto>
        {
            Items = productDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Product> ApplySort(List<Domain.Entities.Product> products, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "name" => isDescending
                ? products.OrderByDescending(p => p.Name).ToList()
                : products.OrderBy(p => p.Name).ToList(),

            "barcode" => isDescending
                ? products.OrderByDescending(p => p.Barcode).ToList()
                : products.OrderBy(p => p.Barcode).ToList(),

            "categoryid" => isDescending
                ? products.OrderByDescending(p => p.CategoryId).ToList()
                : products.OrderBy(p => p.CategoryId).ToList(),

            "createdat" => isDescending
                ? products.OrderByDescending(p => p.CreatedAt).ToList()
                : products.OrderBy(p => p.CreatedAt).ToList(),

            _ => products.OrderByDescending(p => p.CreatedAt).ToList()
        };
    }
}
