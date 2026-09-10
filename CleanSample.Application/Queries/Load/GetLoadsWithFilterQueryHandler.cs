using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Load;

public class GetLoadsWithFilterQueryHandler : IRequestHandler<GetLoadsWithFilterQuery, PaginatedResultDto<LoadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetLoadsWithFilterQueryHandler> _logger;

    public GetLoadsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetLoadsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<LoadDto>> Handle(GetLoadsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetLoadsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var loads = await _unitOfWork.Loads.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            loads = loads.Where(l =>
                (l.Barcode != null && l.Barcode.ToLower().Contains(searchTermLower)) ||
                (l.LoadRequest != null && l.LoadRequest.RequestNumber.ToLower().Contains(searchTermLower)) ||
                (l.Part != null && (l.Part.Code.ToLower().Contains(searchTermLower) || l.Part.Name.ToLower().Contains(searchTermLower))) ||
                (l.Loader != null && l.Loader.FullName.ToLower().Contains(searchTermLower)) ||
                (l.Driver != null && l.Driver.FullName.ToLower().Contains(searchTermLower)) ||
                (l.Vehicle != null && (l.Vehicle.VehicleNumber.ToLower().Contains(searchTermLower) || l.Vehicle.PlateNumber.ToLower().Contains(searchTermLower))) ||
                (l.Notes != null && l.Notes.ToLower().Contains(searchTermLower)) ||
                l.Status.ToLower().Contains(searchTermLower)
            ).ToList();
        }

        if (filter.LoadRequestId.HasValue)
        {
            loads = loads.Where(l => l.LoadRequestId == filter.LoadRequestId.Value).ToList();
        }

        if (filter.LoadRequestPartId.HasValue)
        {
            loads = loads.Where(l => l.LoadRequestPartId == filter.LoadRequestPartId.Value).ToList();
        }

        if (filter.PartId.HasValue)
        {
            loads = loads.Where(l => l.PartId == filter.PartId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Barcode))
        {
            var barcodeLower = filter.Barcode.ToLower();
            loads = loads.Where(l => l.Barcode != null && l.Barcode.ToLower().Contains(barcodeLower)).ToList();
        }

        if (filter.LoadedBy.HasValue)
        {
            loads = loads.Where(l => l.LoadedBy == filter.LoadedBy.Value).ToList();
        }

        if (filter.DriverId.HasValue)
        {
            loads = loads.Where(l => l.DriverId == filter.DriverId.Value).ToList();
        }

        if (filter.VehicleId.HasValue)
        {
            loads = loads.Where(l => l.VehicleId == filter.VehicleId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var statusLower = filter.Status.ToLower();
            loads = loads.Where(l => l.Status.ToLower().Contains(statusLower)).ToList();
        }

        if (filter.FromDate.HasValue)
        {
            loads = loads.Where(l => l.LoadDate >= filter.FromDate.Value).ToList();
        }

        if (filter.ToDate.HasValue)
        {
            loads = loads.Where(l => l.LoadDate <= filter.ToDate.Value).ToList();
        }

        loads = ApplySort(loads.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = loads.Count();

        var paginatedItems = loads
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(l => new LoadDto
        {
            Id = l.Id,
            LoadRequestId = l.LoadRequestId,
            RequestNumber = l.LoadRequest?.RequestNumber,
            LoadRequestPartId = l.LoadRequestPartId,
            PartId = l.PartId,
            PartCode = l.Part?.Code,
            PartName = l.Part?.Name,
            Barcode = l.Barcode,
            Quantity = l.Quantity,
            LoadedBy = l.LoadedBy,
            LoaderName = l.Loader?.FullName,
            DriverId = l.DriverId,
            DriverName = l.Driver?.FullName,
            VehicleId = l.VehicleId,
            VehicleNumber = l.Vehicle?.VehicleNumber,
            PlateNumber = l.Vehicle?.PlateNumber,
            LoadDate = l.LoadDate,
            Status = l.Status,
            Notes = l.Notes,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<LoadDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Load> ApplySort(List<Domain.Entities.Load> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "loaddate" => isDescending
                ? items.OrderByDescending(x => x.LoadDate).ToList()
                : items.OrderBy(x => x.LoadDate).ToList(),

            "loadrequestid" => isDescending
                ? items.OrderByDescending(x => x.LoadRequestId).ToList()
                : items.OrderBy(x => x.LoadRequestId).ToList(),

            "partid" => isDescending
                ? items.OrderByDescending(x => x.PartId).ToList()
                : items.OrderBy(x => x.PartId).ToList(),

            "quantity" => isDescending
                ? items.OrderByDescending(x => x.Quantity).ToList()
                : items.OrderBy(x => x.Quantity).ToList(),

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
