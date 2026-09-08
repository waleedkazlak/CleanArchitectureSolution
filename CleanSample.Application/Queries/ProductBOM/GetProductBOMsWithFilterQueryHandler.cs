using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.ProductBOM;

public class GetProductBOMsWithFilterQueryHandler : IRequestHandler<GetProductBOMsWithFilterQuery, PaginatedResultDto<ProductBOMDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetProductBOMsWithFilterQueryHandler> _logger;

    public GetProductBOMsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetProductBOMsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<ProductBOMDto>> Handle(GetProductBOMsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetProductBOMsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var productBoms = await _unitOfWork.ProductBOMs.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            productBoms = productBoms.Where(pb =>
                (pb.ProductVariant != null && pb.ProductVariant.Code.ToLower().Contains(searchTermLower)) ||
                (pb.Part != null && pb.Part.Code.ToLower().Contains(searchTermLower)) ||
                (pb.Part != null && pb.Part.Name.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        if (filter.ProductVariantId.HasValue)
        {
            productBoms = productBoms.Where(pb => pb.ProductVariantId == filter.ProductVariantId.Value).ToList();
        }

        if (filter.PartId.HasValue)
        {
            productBoms = productBoms.Where(pb => pb.PartId == filter.PartId.Value).ToList();
        }

        if (filter.MinQuantity.HasValue)
        {
            productBoms = productBoms.Where(pb => pb.Quantity >= filter.MinQuantity.Value).ToList();
        }

        if (filter.MaxQuantity.HasValue)
        {
            productBoms = productBoms.Where(pb => pb.Quantity <= filter.MaxQuantity.Value).ToList();
        }

        productBoms = ApplySort(productBoms.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = productBoms.Count();

        var paginatedItems = productBoms
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(pb => new ProductBOMDto
        {
            Id = pb.Id,
            ProductVariantId = pb.ProductVariantId,
            ProductVariantCode = pb.ProductVariant?.Code,
            PartId = pb.PartId,
            PartCode = pb.Part?.Code,
            PartName = pb.Part?.Name,
            Quantity = pb.Quantity,
            CreatedAt = pb.CreatedAt,
            UpdatedAt = pb.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<ProductBOMDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.ProductBOM> ApplySort(List<Domain.Entities.ProductBOM> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "productvariantid" => isDescending
                ? items.OrderByDescending(x => x.ProductVariantId).ToList()
                : items.OrderBy(x => x.ProductVariantId).ToList(),

            "partid" => isDescending
                ? items.OrderByDescending(x => x.PartId).ToList()
                : items.OrderBy(x => x.PartId).ToList(),

            "quantity" => isDescending
                ? items.OrderByDescending(x => x.Quantity).ToList()
                : items.OrderBy(x => x.Quantity).ToList(),

            "createdat" => isDescending
                ? items.OrderByDescending(x => x.CreatedAt).ToList()
                : items.OrderBy(x => x.CreatedAt).ToList(),

            _ => items.OrderByDescending(x => x.CreatedAt).ToList()
        };
    }
}
