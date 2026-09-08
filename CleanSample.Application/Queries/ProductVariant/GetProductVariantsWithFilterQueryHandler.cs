using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.ProductVariant;

public class GetProductVariantsWithFilterQueryHandler : IRequestHandler<GetProductVariantsWithFilterQuery, PaginatedResultDto<ProductVariantDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProductVariantsWithFilterQueryHandler> _logger;

    public GetProductVariantsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductVariantsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<ProductVariantDto>> Handle(GetProductVariantsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetProductVariantsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var variants = await _unitOfWork.ProductVariants.GetAllAsync();

        if (filter.ProductId.HasValue)
        {
            variants = variants.Where(v => v.ProductId == filter.ProductId.Value).ToList();
        }

        if (filter.ColorId.HasValue)
        {
            variants = variants.Where(v => v.ColorId == filter.ColorId.Value).ToList();
        }

        if (filter.MaterialId.HasValue)
        {
            variants = variants.Where(v => v.MaterialId == filter.MaterialId.Value).ToList();
        }

        if (filter.DesignId.HasValue)
        {
            variants = variants.Where(v => v.DesignId == filter.DesignId.Value).ToList();
        }

        if (filter.IsActive.HasValue)
        {
            variants = variants.Where(v => v.IsActive == filter.IsActive.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            variants = variants.Where(v =>
                v.Code.ToLower().Contains(searchTermLower) ||
                (v.Barcode != null && v.Barcode.ToLower().Contains(searchTermLower)) ||
                (v.Description != null && v.Description.ToLower().Contains(searchTermLower)) ||
                (v.Product != null && v.Product.Name.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        variants = ApplySort(variants.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = variants.Count();

        var paginatedVariants = variants
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var variantDtos = paginatedVariants.Select(v => new ProductVariantDto
        {
            Id = v.Id,
            ProductId = v.ProductId,
            ProductName = v.Product?.Name,
            ColorId = v.ColorId,
            ColorName = v.Color?.Name,
            MaterialId = v.MaterialId,
            MaterialName = v.Material?.Name,
            DesignId = v.DesignId,
            DesignName = v.Design?.Name,
            Code = v.Code,
            Barcode = v.Barcode,
            Description = v.Description,
            IsActive = v.IsActive,
            CreatedAt = v.CreatedAt,
            UpdatedAt = v.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<ProductVariantDto>
        {
            Items = variantDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.ProductVariant> ApplySort(List<Domain.Entities.ProductVariant> variants, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "code" => isDescending
                ? variants.OrderByDescending(v => v.Code).ToList()
                : variants.OrderBy(v => v.Code).ToList(),

            "barcode" => isDescending
                ? variants.OrderByDescending(v => v.Barcode).ToList()
                : variants.OrderBy(v => v.Barcode).ToList(),

            "productid" => isDescending
                ? variants.OrderByDescending(v => v.ProductId).ToList()
                : variants.OrderBy(v => v.ProductId).ToList(),

            "createdat" => isDescending
                ? variants.OrderByDescending(v => v.CreatedAt).ToList()
                : variants.OrderBy(v => v.CreatedAt).ToList(),

            _ => variants.OrderByDescending(v => v.CreatedAt).ToList()
        };
    }
}
