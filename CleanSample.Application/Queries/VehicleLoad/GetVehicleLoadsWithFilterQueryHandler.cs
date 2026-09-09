using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.VehicleLoad;

public class GetVehicleLoadsWithFilterQueryHandler : IRequestHandler<GetVehicleLoadsWithFilterQuery, PaginatedResultDto<VehicleLoadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetVehicleLoadsWithFilterQueryHandler> _logger;

    public GetVehicleLoadsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetVehicleLoadsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<VehicleLoadDto>> Handle(GetVehicleLoadsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetVehicleLoadsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var loads = await _unitOfWork.VehicleLoads.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            loads = loads.Where(v =>
                (v.PickRequest != null && v.PickRequest.RequestNumber.ToLower().Contains(searchTermLower)) ||
                (v.Vehicle != null && (v.Vehicle.VehicleNumber.ToLower().Contains(searchTermLower) || v.Vehicle.PlateNumber.ToLower().Contains(searchTermLower))) ||
                (v.Driver != null && v.Driver.FullName.ToLower().Contains(searchTermLower)) ||
                (v.Verifier != null && v.Verifier.FullName.ToLower().Contains(searchTermLower)) ||
                (v.Notes != null && v.Notes.ToLower().Contains(searchTermLower)) ||
                v.Status.ToLower().Contains(searchTermLower)
            ).ToList();
        }

        if (filter.PickRequestId.HasValue)
        {
            loads = loads.Where(v => v.PickRequestId == filter.PickRequestId.Value).ToList();
        }

        if (filter.VehicleId.HasValue)
        {
            loads = loads.Where(v => v.VehicleId == filter.VehicleId.Value).ToList();
        }

        if (filter.DriverId.HasValue)
        {
            loads = loads.Where(v => v.DriverId == filter.DriverId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var statusLower = filter.Status.ToLower();
            loads = loads.Where(v => v.Status.ToLower().Contains(statusLower)).ToList();
        }

        if (filter.Verified.HasValue)
        {
            loads = loads.Where(v => v.Verified == filter.Verified.Value).ToList();
        }

        if (filter.VerifiedBy.HasValue)
        {
            loads = loads.Where(v => v.VerifiedBy == filter.VerifiedBy.Value).ToList();
        }

        if (filter.FromDate.HasValue)
        {
            loads = loads.Where(v => v.LoadDate >= filter.FromDate.Value).ToList();
        }

        if (filter.ToDate.HasValue)
        {
            loads = loads.Where(v => v.LoadDate <= filter.ToDate.Value).ToList();
        }

        loads = ApplySort(loads.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = loads.Count();

        var paginatedItems = loads
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(vl => new VehicleLoadDto
        {
            Id = vl.Id,
            PickRequestId = vl.PickRequestId,
            RequestNumber = vl.PickRequest?.RequestNumber,
            VehicleId = vl.VehicleId,
            VehicleNumber = vl.Vehicle?.VehicleNumber,
            PlateNumber = vl.Vehicle?.PlateNumber,
            DriverId = vl.DriverId,
            DriverName = vl.Driver?.FullName,
            LoadDate = vl.LoadDate,
            Status = vl.Status,
            Verified = vl.Verified,
            VerifiedBy = vl.VerifiedBy,
            VerifierName = vl.Verifier?.FullName,
            VerifiedAt = vl.VerifiedAt,
            Notes = vl.Notes,
            CreatedAt = vl.CreatedAt,
            UpdatedAt = vl.UpdatedAt,
            VehicleLoadItems = vl.VehicleLoadItems?.Select(i => new VehicleLoadItemDto
            {
                Id = i.Id,
                VehicleLoadId = i.VehicleLoadId,
                PickId = i.PickId,
                PartId = i.PartId,
                PartCode = i.Part?.Code,
                PartName = i.Part?.Name,
                Barcode = i.Barcode,
                Quantity = i.Quantity,
                LoadedAt = i.LoadedAt,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            }).ToList() ?? new List<VehicleLoadItemDto>()
        }).ToList();

        return new PaginatedResultDto<VehicleLoadDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.VehicleLoad> ApplySort(List<Domain.Entities.VehicleLoad> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "loaddate" => isDescending
                ? items.OrderByDescending(x => x.LoadDate).ToList()
                : items.OrderBy(x => x.LoadDate).ToList(),

            "pickrequestid" => isDescending
                ? items.OrderByDescending(x => x.PickRequestId).ToList()
                : items.OrderBy(x => x.PickRequestId).ToList(),

            "vehicleid" => isDescending
                ? items.OrderByDescending(x => x.VehicleId).ToList()
                : items.OrderBy(x => x.VehicleId).ToList(),

            "driverid" => isDescending
                ? items.OrderByDescending(x => x.DriverId).ToList()
                : items.OrderBy(x => x.DriverId).ToList(),

            "status" => isDescending
                ? items.OrderByDescending(x => x.Status).ToList()
                : items.OrderBy(x => x.Status).ToList(),

            "createdat" => isDescending
                ? items.OrderByDescending(x => x.CreatedAt).ToList()
                : items.OrderBy(x => x.CreatedAt).ToList(),

            _ => items.OrderByDescending(x => x.LoadDate).ToList()
        };
    }
}
