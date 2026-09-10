using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
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

        var loadRequests = await _unitOfWork.LoadRequests.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            loadRequests = loadRequests.Where(p =>
                p.RequestNumber.ToLower().Contains(searchTermLower) ||
                (p.Order != null && p.Order.OrderNumber.ToLower().Contains(searchTermLower)) ||
                (p.Client != null && p.Client.Name.ToLower().Contains(searchTermLower)) ||
                (p.ClientLocation != null && p.ClientLocation.Name.ToLower().Contains(searchTermLower)) ||
                (p.Requester != null && p.Requester.FullName.ToLower().Contains(searchTermLower)) ||
                (p.Driver != null && p.Driver.FullName.ToLower().Contains(searchTermLower)) ||
                (p.Vehicle != null && (p.Vehicle.VehicleNumber.ToLower().Contains(searchTermLower) || p.Vehicle.PlateNumber.ToLower().Contains(searchTermLower))) ||
                (p.DestinationAddress != null && p.DestinationAddress.ToLower().Contains(searchTermLower)) ||
                (p.DestinationCity != null && p.DestinationCity.ToLower().Contains(searchTermLower)) ||
                (p.Description != null && p.Description.ToLower().Contains(searchTermLower)) ||
                p.Status.ToLower().Contains(searchTermLower)
            ).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.RequestNumber))
        {
            var requestNumberLower = filter.RequestNumber.ToLower();
            loadRequests = loadRequests.Where(p => p.RequestNumber.ToLower().Contains(requestNumberLower)).ToList();
        }

        if (filter.OrderId.HasValue)
        {
            loadRequests = loadRequests.Where(p => p.OrderId == filter.OrderId.Value).ToList();
        }

        if (filter.ClientId.HasValue)
        {
            loadRequests = loadRequests.Where(p => p.ClientId == filter.ClientId.Value).ToList();
        }

        if (filter.ClientLocationId.HasValue)
        {
            loadRequests = loadRequests.Where(p => p.ClientLocationId == filter.ClientLocationId.Value).ToList();
        }

        if (filter.RequestedBy.HasValue)
        {
            loadRequests = loadRequests.Where(p => p.RequestedBy == filter.RequestedBy.Value).ToList();
        }

        if (filter.DriverId.HasValue)
        {
            loadRequests = loadRequests.Where(p => p.DriverId == filter.DriverId.Value).ToList();
        }

        if (filter.VehicleId.HasValue)
        {
            loadRequests = loadRequests.Where(p => p.VehicleId == filter.VehicleId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var statusLower = filter.Status.ToLower();
            loadRequests = loadRequests.Where(p => p.Status.ToLower().Contains(statusLower)).ToList();
        }

        if (filter.Verified.HasValue)
        {
            loadRequests = loadRequests.Where(p => p.Verified == filter.Verified.Value).ToList();
        }

        if (filter.FromDate.HasValue)
        {
            loadRequests = loadRequests.Where(p => p.RequestDate >= filter.FromDate.Value).ToList();
        }

        if (filter.ToDate.HasValue)
        {
            loadRequests = loadRequests.Where(p => p.RequestDate <= filter.ToDate.Value).ToList();
        }

        loadRequests = ApplySort(loadRequests.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = loadRequests.Count();

        var paginatedItems = loadRequests
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(p => new LoadRequestDto
        {
            Id = p.Id,
            RequestNumber = p.RequestNumber,
            OrderId = p.OrderId,
            OrderNumber = p.Order?.OrderNumber,
            ClientId = p.ClientId,
            ClientName = p.Client?.Name,
            ClientLocationId = p.ClientLocationId,
            ClientLocationName = p.ClientLocation?.Name,
            RequestedBy = p.RequestedBy,
            RequesterName = p.Requester?.FullName,
            RequestDate = p.RequestDate,
            ExecutionDate = p.ExecutionDate,
            Status = p.Status,
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
                RequestNumber = p.RequestNumber,
                ProductVariantId = l.ProductVariantId,
                ProductVariantCode = l.ProductVariant?.Code,
                Quantity = l.Quantity,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                LoadRequestParts = l.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
                {
                    Id = lrp.Id,
                    LoadRequestId = lrp.LoadRequestId,
                    LoadRequestLineId = lrp.LoadRequestLineId,
                    PartId = lrp.PartId,
                    PartCode = lrp.Part?.Code,
                    PartName = lrp.Part?.Name,
                    RequiredQuantity = lrp.RequiredQuantity,
                    LoadedQuantity = lrp.LoadedQuantity,
                    Status = lrp.Status,
                    CreatedAt = lrp.CreatedAt,
                    UpdatedAt = lrp.UpdatedAt
                }).ToList() ?? new List<LoadRequestPartDto>()
            }).ToList() ?? new List<LoadRequestLineDto>(),
            LoadRequestParts = p.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
            {
                Id = lrp.Id,
                LoadRequestId = lrp.LoadRequestId,
                LoadRequestLineId = lrp.LoadRequestLineId,
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

        return new PaginatedResultDto<LoadRequestDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.LoadRequest> ApplySort(List<Domain.Entities.LoadRequest> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "requestnumber" => isDescending
                ? items.OrderByDescending(x => x.RequestNumber).ToList()
                : items.OrderBy(x => x.RequestNumber).ToList(),

            "clientid" => isDescending
                ? items.OrderByDescending(x => x.ClientId).ToList()
                : items.OrderBy(x => x.ClientId).ToList(),

            "orderid" => isDescending
                ? items.OrderByDescending(x => x.OrderId).ToList()
                : items.OrderBy(x => x.OrderId).ToList(),

            "requestdate" => isDescending
                ? items.OrderByDescending(x => x.RequestDate).ToList()
                : items.OrderBy(x => x.RequestDate).ToList(),

            "executiondate" => isDescending
                ? items.OrderByDescending(x => x.ExecutionDate).ToList()
                : items.OrderBy(x => x.ExecutionDate).ToList(),

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
