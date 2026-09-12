using CleanSample.Application.DTOs;
using CleanSample.Domain.Enums;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.LoadRequestLine;

public class GetLoadRequestLinesWithFilterQueryHandler : IRequestHandler<GetLoadRequestLinesWithFilterQuery, PaginatedResultDto<LoadRequestLineDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetLoadRequestLinesWithFilterQueryHandler> _logger;

    public GetLoadRequestLinesWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetLoadRequestLinesWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<LoadRequestLineDto>> Handle(GetLoadRequestLinesWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetLoadRequestLinesWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var query = _unitOfWork.LoadRequestLines.GetQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            query = query.Where(l =>
                (l.Product != null && l.Product.Name.ToLower().Contains(searchTermLower)) ||
                (l.LoadRequest != null && l.LoadRequest.Description != null && l.LoadRequest.Description.ToLower().Contains(searchTermLower))
            );
        }

        if (filter.LoadRequestId.HasValue)
        {
            query = query.Where(l => l.LoadRequestId == filter.LoadRequestId.Value);
        }

        if (filter.DriverId.HasValue)
        {
            query = query.Where(l => l.LoadRequest != null && l.LoadRequest.DriverId == filter.DriverId.Value);
        }

        if (filter.ApprovedOnly == true)
        {
            query = query.Where(l => l.LoadRequest != null
                && l.LoadRequest.Status != (int)LoadRequestStatusEnum.Cancelled
                && (l.LoadRequest.OrderId == null || (
                    l.LoadRequest.Order != null && (
                        l.LoadRequest.Order.Status == (int)OrderStatusEnum.Approved ||
                        l.LoadRequest.Order.Status == (int)OrderStatusEnum.Processing ||
                        l.LoadRequest.Order.Status == (int)OrderStatusEnum.Completed
                    )
                )));
        }

        if (filter.ProductId.HasValue)
        {
            query = query.Where(l => l.ProductId == filter.ProductId.Value);
        }

        if (filter.MinQuantity.HasValue)
        {
            query = query.Where(l => l.Quantity >= filter.MinQuantity.Value);
        }

        if (filter.MaxQuantity.HasValue)
        {
            query = query.Where(l => l.Quantity <= filter.MaxQuantity.Value);
        }

        var isDescending = filter.SortDirection?.ToLower() == "desc";
        query = (filter.SortBy?.ToLower()) switch
        {
            "loadrequestid" => isDescending ? query.OrderByDescending(x => x.LoadRequestId) : query.OrderBy(x => x.LoadRequestId),
            "productid" => isDescending ? query.OrderByDescending(x => x.ProductId) : query.OrderBy(x => x.ProductId),
            "productname" => isDescending ? query.OrderByDescending(x => x.Product != null ? x.Product.Name : string.Empty) : query.OrderBy(x => x.Product != null ? x.Product.Name : string.Empty),
            "quantity" => isDescending ? query.OrderByDescending(x => x.Quantity) : query.OrderBy(x => x.Quantity),
            "createdat" => isDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var paginatedItems = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var dtos = paginatedItems.Select(line => new LoadRequestLineDto
        {
            Id = line.Id,
            LoadRequestId = line.LoadRequestId,
            ProductId = line.ProductId,
            ProductName = line.Product?.Name,
            Quantity = line.Quantity,
            CreatedAt = line.CreatedAt,
            UpdatedAt = line.UpdatedAt,
            LoadRequestParts = line.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
            {
                Id = lrp.Id,
                LoadRequestId = lrp.LoadRequestId,
                LoadRequestLineId = lrp.LoadRequestLineId,
                ProductId = lrp.ProductId,
                ProductName = lrp.Product?.Name ?? line.Product?.Name,
                PartId = lrp.PartId,
                PartCode = lrp.Part?.Code,
                PartName = lrp.Part?.Name,
                RequiredQuantity = lrp.RequiredQuantity,
                LoadedQuantity = lrp.LoadedQuantity,
                CreatedAt = lrp.CreatedAt,
                UpdatedAt = lrp.UpdatedAt
            }).ToList() ?? new List<LoadRequestPartDto>()
        }).ToList();

        return new PaginatedResultDto<LoadRequestLineDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
