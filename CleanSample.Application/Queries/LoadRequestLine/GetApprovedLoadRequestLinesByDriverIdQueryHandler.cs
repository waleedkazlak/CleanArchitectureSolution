using CleanSample.Application.DTOs;
using CleanSample.Domain.Enums;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.LoadRequestLine;

public class GetApprovedLoadRequestLinesByDriverIdQueryHandler : IRequestHandler<GetApprovedLoadRequestLinesByDriverIdQuery, PaginatedResultDto<LoadRequestLineDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetApprovedLoadRequestLinesByDriverIdQueryHandler> _logger;

    public GetApprovedLoadRequestLinesByDriverIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetApprovedLoadRequestLinesByDriverIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<LoadRequestLineDto>> Handle(
        GetApprovedLoadRequestLinesByDriverIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Retrieving approved load request lines for DriverId: {DriverId} - Page: {PageNumber}, PageSize: {PageSize}",
            request.DriverId, request.PageNumber, request.PageSize);

        var pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
        var pageSize = request.PageSize > 0 && request.PageSize <= 100 ? request.PageSize : 10;

        // Base query with navigation properties
        var query = _unitOfWork.LoadRequestLines.GetQueryable();

        // 1. Filter by DriverId
        query = query.Where(l => l.LoadRequest != null && l.LoadRequest.DriverId == request.DriverId);

        // 2. Filter for Approved Load Requests only:
        //    - Load request itself is not cancelled
        //    - If associated with an Order, the Order must be Approved (3), Processing (4), or Completed (5)
        query = query.Where(l => l.LoadRequest != null
            && l.LoadRequest.Status != (int)LoadRequestStatusEnum.Cancelled
            && (l.LoadRequest.OrderId == null || (
                l.LoadRequest.Order != null && (
                    l.LoadRequest.Order.Status == (int)OrderStatusEnum.Approved ||
                    l.LoadRequest.Order.Status == (int)OrderStatusEnum.Processing ||
                    l.LoadRequest.Order.Status == (int)OrderStatusEnum.Completed
                )
            )));

        // 3. Search term filter
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(l =>
                (l.Product != null && l.Product.Name.ToLower().Contains(term)) ||
                (l.LoadRequest != null && l.LoadRequest.DestinationCity != null && l.LoadRequest.DestinationCity.ToLower().Contains(term)) ||
                (l.LoadRequest != null && l.LoadRequest.Description != null && l.LoadRequest.Description.ToLower().Contains(term)));
        }

        // 4. Sorting
        var isDescending = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase);
        query = (request.SortBy?.ToLower()) switch
        {
            "loadrequestid" => isDescending ? query.OrderByDescending(x => x.LoadRequestId) : query.OrderBy(x => x.LoadRequestId),
            "productid" => isDescending ? query.OrderByDescending(x => x.ProductId) : query.OrderBy(x => x.ProductId),
            "productname" => isDescending ? query.OrderByDescending(x => x.Product != null ? x.Product.Name : string.Empty) : query.OrderBy(x => x.Product != null ? x.Product.Name : string.Empty),
            "quantity" => isDescending ? query.OrderByDescending(x => x.Quantity) : query.OrderBy(x => x.Quantity),
            "createdat" => isDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        // 5. Total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // 6. Pagination on database side
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // 7. Map to DTOs
        var dtos = items.Select(line => new LoadRequestLineDto
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
