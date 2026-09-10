using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.VehicleOffload;

public class GetVehicleOffloadsWithFilterQueryHandler : IRequestHandler<GetVehicleOffloadsWithFilterQuery, PaginatedResultDto<VehicleOffloadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetVehicleOffloadsWithFilterQueryHandler> _logger;

    public GetVehicleOffloadsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetVehicleOffloadsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<VehicleOffloadDto>> Handle(GetVehicleOffloadsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetVehicleOffloadsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var offloads = await _unitOfWork.VehicleOffloads.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            offloads = offloads.Where(v =>
                (v.LoadRequest != null && v.LoadRequest.RequestNumber.ToLower().Contains(searchTermLower)) ||
                (v.Vehicle != null && (v.Vehicle.VehicleNumber.ToLower().Contains(searchTermLower) || v.Vehicle.PlateNumber.ToLower().Contains(searchTermLower))) ||
                (v.Driver != null && v.Driver.FullName.ToLower().Contains(searchTermLower)) ||
                (v.Verifier != null && v.Verifier.FullName.ToLower().Contains(searchTermLower)) ||
                (v.Notes != null && v.Notes.ToLower().Contains(searchTermLower)) ||
                v.Status.ToLower().Contains(searchTermLower)
            ).ToList();
        }

        if (filter.LoadRequestId.HasValue)
        {
            offloads = offloads.Where(v => v.LoadRequestId == filter.LoadRequestId.Value).ToList();
        }

        if (filter.VehicleId.HasValue)
        {
            offloads = offloads.Where(v => v.VehicleId == filter.VehicleId.Value).ToList();
        }

        if (filter.DriverId.HasValue)
        {
            offloads = offloads.Where(v => v.DriverId == filter.DriverId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var statusLower = filter.Status.ToLower();
            offloads = offloads.Where(v => v.Status.ToLower().Contains(statusLower)).ToList();
        }

        if (filter.Verified.HasValue)
        {
            offloads = offloads.Where(v => v.Verified == filter.Verified.Value).ToList();
        }

        if (filter.VerifiedBy.HasValue)
        {
            offloads = offloads.Where(v => v.VerifiedBy == filter.VerifiedBy.Value).ToList();
        }

        if (filter.FromDate.HasValue)
        {
            offloads = offloads.Where(v => v.OffloadDate >= filter.FromDate.Value).ToList();
        }

        if (filter.ToDate.HasValue)
        {
            offloads = offloads.Where(v => v.OffloadDate <= filter.ToDate.Value).ToList();
        }

        offloads = ApplySort(offloads.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = offloads.Count();

        var paginatedItems = offloads
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(vo => new VehicleOffloadDto
        {
            Id = vo.Id,
            LoadRequestId = vo.LoadRequestId,
            RequestNumber = vo.LoadRequest?.RequestNumber,
            VehicleId = vo.VehicleId,
            VehicleNumber = vo.Vehicle?.VehicleNumber,
            PlateNumber = vo.Vehicle?.PlateNumber,
            DriverId = vo.DriverId,
            DriverName = vo.Driver?.FullName,
            OffloadDate = vo.OffloadDate,
            Status = vo.Status,
            Verified = vo.Verified,
            VerifiedBy = vo.VerifiedBy,
            VerifierName = vo.Verifier?.FullName,
            VerifiedAt = vo.VerifiedAt,
            Notes = vo.Notes,
            CreatedAt = vo.CreatedAt,
            UpdatedAt = vo.UpdatedAt,
            VehicleOffloadItems = vo.VehicleOffloadItems?.Select(i => new VehicleOffloadItemDto
            {
                Id = i.Id,
                VehicleOffloadId = i.VehicleOffloadId,
                LoadId = i.LoadId,
                PartId = i.PartId,
                PartCode = i.Part?.Code,
                PartName = i.Part?.Name,
                Barcode = i.Barcode,
                Quantity = i.Quantity,
                OffloadedAt = i.OffloadedAt,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt
            }).ToList() ?? new List<VehicleOffloadItemDto>()
        }).ToList();

        return new PaginatedResultDto<VehicleOffloadDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.VehicleOffload> ApplySort(List<Domain.Entities.VehicleOffload> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "offloaddate" => isDescending
                ? items.OrderByDescending(x => x.OffloadDate).ToList()
                : items.OrderBy(x => x.OffloadDate).ToList(),

            "loadrequestid" => isDescending
                ? items.OrderByDescending(x => x.LoadRequestId).ToList()
                : items.OrderBy(x => x.LoadRequestId).ToList(),

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

            _ => items.OrderByDescending(x => x.OffloadDate).ToList()
        };
    }
}
