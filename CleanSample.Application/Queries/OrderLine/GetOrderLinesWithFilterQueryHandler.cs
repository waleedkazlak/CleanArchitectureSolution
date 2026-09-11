using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.OrderLine;

public class GetOrderLinesWithFilterQueryHandler : IRequestHandler<GetOrderLinesWithFilterQuery, PaginatedResultDto<OrderLineDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetOrderLinesWithFilterQueryHandler> _logger;

    public GetOrderLinesWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetOrderLinesWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<OrderLineDto>> Handle(GetOrderLinesWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetOrderLinesWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var orderLines = await _unitOfWork.OrderLines.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            orderLines = orderLines.Where(ol =>
                (ol.Product != null && ol.Product.Name.ToLower().Contains(searchTermLower)) ||
                (ol.Notes != null && ol.Notes.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        if (filter.OrderId.HasValue)
        {
            orderLines = orderLines.Where(ol => ol.OrderId == filter.OrderId.Value).ToList();
        }

        if (filter.ProductId.HasValue)
        {
            orderLines = orderLines.Where(ol => ol.ProductId == filter.ProductId.Value).ToList();
        }

        if (filter.MinQuantity.HasValue)
        {
            orderLines = orderLines.Where(ol => ol.Quantity >= filter.MinQuantity.Value).ToList();
        }

        if (filter.MaxQuantity.HasValue)
        {
            orderLines = orderLines.Where(ol => ol.Quantity <= filter.MaxQuantity.Value).ToList();
        }

        orderLines = ApplySort(orderLines.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = orderLines.Count();

        var paginatedItems = orderLines
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(ol => new OrderLineDto
        {
            Id = ol.Id,
            OrderId = ol.OrderId,
            ProductId = ol.ProductId,
            ProductName = ol.Product?.Name,
            Quantity = ol.Quantity,
            Notes = ol.Notes,
            CreatedAt = ol.CreatedAt,
            UpdatedAt = ol.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<OrderLineDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.OrderLine> ApplySort(List<Domain.Entities.OrderLine> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "orderid" => isDescending
                ? items.OrderByDescending(x => x.OrderId).ToList()
                : items.OrderBy(x => x.OrderId).ToList(),

            "productid" => isDescending
                ? items.OrderByDescending(x => x.ProductId).ToList()
                : items.OrderBy(x => x.ProductId).ToList(),

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
