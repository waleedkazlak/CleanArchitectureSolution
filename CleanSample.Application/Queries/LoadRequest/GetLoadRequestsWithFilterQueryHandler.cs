using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.LoadRequest;

public class GetLoadRequestsWithFilterQueryHandler : IRequestHandler<GetLoadRequestsWithFilterQuery, PaginatedResultDto<LoadRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetLoadRequestsWithFilterQueryHandler> _logger;

    public GetLoadRequestsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetLoadRequestsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<LoadRequestDto>> Handle(GetLoadRequestsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetLoadRequestsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var query = _unitOfWork.LoadRequests.GetQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTerm = filter.SearchTerm.Trim();
            query = query.Where(p =>
                (p.Client != null && p.Client.Name.Contains(searchTerm)) ||
                (p.ClientLocation != null && p.ClientLocation.Name.Contains(searchTerm)) ||
                (p.Requester != null && p.Requester.FullName.Contains(searchTerm)) ||
                (p.Driver != null && p.Driver.FullName.Contains(searchTerm)) ||
                (p.Vehicle != null && (p.Vehicle.VehicleNumber.Contains(searchTerm) || p.Vehicle.PlateNumber.Contains(searchTerm))) ||
                (p.DestinationAddress != null && p.DestinationAddress.Contains(searchTerm)) ||
                (p.DestinationCity != null && p.DestinationCity.Contains(searchTerm)) ||
                (p.Description != null && p.Description.Contains(searchTerm))
            );
        }

        if (filter.OrderId.HasValue)
        {
            query = query.Where(p => p.OrderId == filter.OrderId.Value);
        }

        if (filter.ClientId.HasValue)
        {
            query = query.Where(p => p.ClientId == filter.ClientId.Value);
        }

        if (filter.ClientLocationId.HasValue)
        {
            query = query.Where(p => p.ClientLocationId == filter.ClientLocationId.Value);
        }

        if (filter.RequestedBy.HasValue)
        {
            query = query.Where(p => p.RequestedBy == filter.RequestedBy.Value);
        }

        if (filter.DriverId.HasValue)
        {
            query = query.Where(p => p.DriverId == filter.DriverId.Value);
        }

        if (filter.VehicleId.HasValue)
        {
            query = query.Where(p => p.VehicleId == filter.VehicleId.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(p => p.Status == filter.Status.Value);
        }

        if (filter.Verified.HasValue)
        {
            query = query.Where(p => p.Verified == filter.Verified.Value);
        }

        if (filter.FromDate.HasValue)
        {
            query = query.Where(p => p.RequestDate >= filter.FromDate.Value);
        }

        if (filter.ToDate.HasValue)
        {
            query = query.Where(p => p.RequestDate <= filter.ToDate.Value);
        }

        query = ApplySort(query, filter.SortBy, filter.SortDirection);

        var totalCount = await query.CountAsync(cancellationToken);

        var paginatedItems = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var dtos = paginatedItems.Select(p => new LoadRequestDto
        {
            Id = p.Id,
            OrderId = p.OrderId,
            ClientId = p.ClientId,
            ClientName = p.Client?.Name,
            ClientLocationId = p.ClientLocationId,
            ClientLocationName = p.ClientLocation?.Name,
            RequestedBy = p.RequestedBy,
            RequesterName = p.Requester?.FullName,
            RequestDate = p.RequestDate,
            ExecutionDate = p.ExecutionDate,
            Status = p.Status,
            StatusName = p.LoadRequestStatus?.Name,
            DestinationAddress = p.DestinationAddress,
            DestinationCity = p.DestinationCity,
            Description = p.Description,
            DriverId = p.DriverId,
            DriverName = p.Driver?.FullName,
            VehicleId = p.VehicleId,
            VehicleNumber = p.Vehicle?.VehicleNumber,
            PlateNumber = p.Vehicle?.PlateNumber,
            Verified = p.Verified,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            LoadRequestLines = p.LoadRequestLines?.Select(l => new LoadRequestLineDto
            {
                Id = l.Id,
                LoadRequestId = l.LoadRequestId,
                ProductId = l.ProductId,
                ProductName = l.Product?.Name,
                Quantity = l.Quantity,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                LoadRequestParts = l.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
                {
                    Id = lrp.Id,
                    LoadRequestId = lrp.LoadRequestId,
                    LoadRequestLineId = lrp.LoadRequestLineId,
                    ProductId = lrp.ProductId,
                    ProductName = lrp.Product?.Name ?? l.Product?.Name,
                    PartId = lrp.PartId,
                    PartCode = lrp.Part?.Code,
                    PartName = lrp.Part?.Name,
                    RequiredQuantity = lrp.RequiredQuantity,
                    LoadedQuantity = lrp.LoadedQuantity,
                    CreatedAt = lrp.CreatedAt,
                    UpdatedAt = lrp.UpdatedAt
                }).ToList() ?? new List<LoadRequestPartDto>()
            }).ToList() ?? new List<LoadRequestLineDto>(),
            LoadRequestParts = p.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
            {
                Id = lrp.Id,
                LoadRequestId = lrp.LoadRequestId,
                LoadRequestLineId = lrp.LoadRequestLineId,
                ProductId = lrp.ProductId,
                ProductName = lrp.Product?.Name,
                PartId = lrp.PartId,
                PartCode = lrp.Part?.Code,
                PartName = lrp.Part?.Name,
                RequiredQuantity = lrp.RequiredQuantity,
                LoadedQuantity = lrp.LoadedQuantity,
                CreatedAt = lrp.CreatedAt,
                UpdatedAt = lrp.UpdatedAt
            }).ToList() ?? new List<LoadRequestPartDto>()
        }).ToList();

        return new PaginatedResultDto<LoadRequestDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private static IQueryable<Domain.Entities.LoadRequest> ApplySort(
        IQueryable<Domain.Entities.LoadRequest> query,
        string? sortBy,
        string? sortDirection)
    {
        var isDescending = string.Equals(sortDirection, "desc", StringComparison.OrdinalIgnoreCase);

        return (sortBy?.ToLower()) switch
        {
            "clientid" => isDescending ? query.OrderByDescending(x => x.ClientId) : query.OrderBy(x => x.ClientId),
            "orderid" => isDescending ? query.OrderByDescending(x => x.OrderId) : query.OrderBy(x => x.OrderId),
            "requestdate" => isDescending ? query.OrderByDescending(x => x.RequestDate) : query.OrderBy(x => x.RequestDate),
            "executiondate" => isDescending ? query.OrderByDescending(x => x.ExecutionDate) : query.OrderBy(x => x.ExecutionDate),
            "status" => isDescending ? query.OrderByDescending(x => x.Status) : query.OrderBy(x => x.Status),
            "createdat" => isDescending ? query.OrderByDescending(x => x.CreatedAt) : query.OrderBy(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.CreatedAt)
        };
    }
}
