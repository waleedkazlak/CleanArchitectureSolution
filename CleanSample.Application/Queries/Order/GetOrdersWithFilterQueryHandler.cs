using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Order;

public class GetOrdersWithFilterQueryHandler : IRequestHandler<GetOrdersWithFilterQuery, PaginatedResultDto<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetOrdersWithFilterQueryHandler> _logger;

    public GetOrdersWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetOrdersWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<OrderDto>> Handle(GetOrdersWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetOrdersWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var orders = await _unitOfWork.Orders.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            orders = orders.Where(o =>
                (o.Client != null && o.Client.Name.ToLower().Contains(searchTermLower)) ||
                o.Status.ToLower().Contains(searchTermLower) ||
                (o.Notes != null && o.Notes.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        if (filter.ClientId.HasValue)
        {
            orders = orders.Where(o => o.ClientId == filter.ClientId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var statusLower = filter.Status.ToLower();
            orders = orders.Where(o => o.Status.ToLower().Contains(statusLower)).ToList();
        }

        if (filter.FromDate.HasValue)
        {
            orders = orders.Where(o => o.OrderDate >= filter.FromDate.Value).ToList();
        }

        if (filter.ToDate.HasValue)
        {
            orders = orders.Where(o => o.OrderDate <= filter.ToDate.Value).ToList();
        }

        orders = ApplySort(orders.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = orders.Count();

        var paginatedItems = orders
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(o => new OrderDto
        {
            Id = o.Id,
            ClientId = o.ClientId,
            ClientName = o.Client?.Name,
            OrderDate = o.OrderDate,
            RequiredDate = o.RequiredDate,
            Status = o.Status,
            Notes = o.Notes,
            CreatedAt = o.CreatedAt,
            UpdatedAt = o.UpdatedAt,
            OrderLines = o.OrderLines?.Select(ol => new OrderLineDto
            {
                Id = ol.Id,
                OrderId = ol.OrderId,
                ProductId = ol.ProductId,
                ProductName = ol.Product?.Name,
                Quantity = ol.Quantity,
                Notes = ol.Notes,
                CreatedAt = ol.CreatedAt,
                UpdatedAt = ol.UpdatedAt
            }).ToList() ?? new List<OrderLineDto>()
        }).ToList();

        return new PaginatedResultDto<OrderDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Order> ApplySort(List<Domain.Entities.Order> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {

            "clientid" => isDescending
                ? items.OrderByDescending(x => x.ClientId).ToList()
                : items.OrderBy(x => x.ClientId).ToList(),

            "orderdate" => isDescending
                ? items.OrderByDescending(x => x.OrderDate).ToList()
                : items.OrderBy(x => x.OrderDate).ToList(),

            "requireddate" => isDescending
                ? items.OrderByDescending(x => x.RequiredDate).ToList()
                : items.OrderBy(x => x.RequiredDate).ToList(),

            "status" => isDescending
                ? items.OrderByDescending(x => x.Status).ToList()
                : items.OrderBy(x => x.Status).ToList(),

            "createdat" => isDescending
                ? items.OrderByDescending(x => x.CreatedAt).ToList()
                : items.OrderBy(x => x.CreatedAt).ToList(),

            _ => items.OrderByDescending(x => x.CreatedAt).ToList()
        };
    }
}
