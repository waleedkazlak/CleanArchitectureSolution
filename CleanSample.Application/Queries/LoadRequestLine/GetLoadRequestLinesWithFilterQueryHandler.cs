using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
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

        var lines = await _unitOfWork.LoadRequestLines.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            lines = lines.Where(l =>
                (l.Product != null && l.Product.Name.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        if (filter.LoadRequestId.HasValue)
        {
            lines = lines.Where(l => l.LoadRequestId == filter.LoadRequestId.Value).ToList();
        }

        if (filter.ProductId.HasValue)
        {
            lines = lines.Where(l => l.ProductId == filter.ProductId.Value).ToList();
        }

        if (filter.MinQuantity.HasValue)
        {
            lines = lines.Where(l => l.Quantity >= filter.MinQuantity.Value).ToList();
        }

        if (filter.MaxQuantity.HasValue)
        {
            lines = lines.Where(l => l.Quantity <= filter.MaxQuantity.Value).ToList();
        }

        lines = ApplySort(lines.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = lines.Count();

        var paginatedItems = lines
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

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
                Status = lrp.Status,
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

    private List<Domain.Entities.LoadRequestLine> ApplySort(List<Domain.Entities.LoadRequestLine> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "loadrequestid" => isDescending
                ? items.OrderByDescending(x => x.LoadRequestId).ToList()
                : items.OrderBy(x => x.LoadRequestId).ToList(),

            "productid" => isDescending
                ? items.OrderByDescending(x => x.ProductId).ToList()
                : items.OrderBy(x => x.ProductId).ToList(),

            "productname" => isDescending
                ? items.OrderByDescending(x => x.Product?.Name).ToList()
                : items.OrderBy(x => x.Product?.Name).ToList(),

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
