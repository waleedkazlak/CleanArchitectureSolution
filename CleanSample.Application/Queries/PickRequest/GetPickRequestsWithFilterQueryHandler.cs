using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.PickRequest;

public class GetPickRequestsWithFilterQueryHandler : IRequestHandler<GetPickRequestsWithFilterQuery, PaginatedResultDto<PickRequestDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPickRequestsWithFilterQueryHandler> _logger;

    public GetPickRequestsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPickRequestsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<PickRequestDto>> Handle(GetPickRequestsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetPickRequestsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var pickRequests = await _unitOfWork.PickRequests.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            pickRequests = pickRequests.Where(p =>
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
            pickRequests = pickRequests.Where(p => p.RequestNumber.ToLower().Contains(requestNumberLower)).ToList();
        }

        if (filter.OrderId.HasValue)
        {
            pickRequests = pickRequests.Where(p => p.OrderId == filter.OrderId.Value).ToList();
        }

        if (filter.ClientId.HasValue)
        {
            pickRequests = pickRequests.Where(p => p.ClientId == filter.ClientId.Value).ToList();
        }

        if (filter.ClientLocationId.HasValue)
        {
            pickRequests = pickRequests.Where(p => p.ClientLocationId == filter.ClientLocationId.Value).ToList();
        }

        if (filter.RequestedBy.HasValue)
        {
            pickRequests = pickRequests.Where(p => p.RequestedBy == filter.RequestedBy.Value).ToList();
        }

        if (filter.DriverId.HasValue)
        {
            pickRequests = pickRequests.Where(p => p.DriverId == filter.DriverId.Value).ToList();
        }

        if (filter.VehicleId.HasValue)
        {
            pickRequests = pickRequests.Where(p => p.VehicleId == filter.VehicleId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var statusLower = filter.Status.ToLower();
            pickRequests = pickRequests.Where(p => p.Status.ToLower().Contains(statusLower)).ToList();
        }

        if (filter.Verified.HasValue)
        {
            pickRequests = pickRequests.Where(p => p.Verified == filter.Verified.Value).ToList();
        }

        if (filter.FromDate.HasValue)
        {
            pickRequests = pickRequests.Where(p => p.RequestDate >= filter.FromDate.Value).ToList();
        }

        if (filter.ToDate.HasValue)
        {
            pickRequests = pickRequests.Where(p => p.RequestDate <= filter.ToDate.Value).ToList();
        }

        pickRequests = ApplySort(pickRequests.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = pickRequests.Count();

        var paginatedItems = pickRequests
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(p => new PickRequestDto
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
            PickRequestLines = p.PickRequestLines?.Select(l => new PickRequestLineDto
            {
                Id = l.Id,
                PickRequestId = l.PickRequestId,
                RequestNumber = p.RequestNumber,
                ProductVariantId = l.ProductVariantId,
                ProductVariantCode = l.ProductVariant?.Code,
                Quantity = l.Quantity,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt
            }).ToList() ?? new List<PickRequestLineDto>()
        }).ToList();

        return new PaginatedResultDto<PickRequestDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.PickRequest> ApplySort(List<Domain.Entities.PickRequest> items, string? sortBy, string? sortDirection)
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
