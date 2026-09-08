using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Pick;

public class GetPicksWithFilterQueryHandler : IRequestHandler<GetPicksWithFilterQuery, PaginatedResultDto<PickDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPicksWithFilterQueryHandler> _logger;

    public GetPicksWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPicksWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<PickDto>> Handle(GetPicksWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetPicksWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var picks = await _unitOfWork.Picks.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            picks = picks.Where(p =>
                (p.Barcode != null && p.Barcode.ToLower().Contains(searchTermLower)) ||
                (p.PickRequest != null && p.PickRequest.RequestNumber.ToLower().Contains(searchTermLower)) ||
                (p.Part != null && (p.Part.Code.ToLower().Contains(searchTermLower) || p.Part.Name.ToLower().Contains(searchTermLower))) ||
                (p.Picker != null && p.Picker.FullName.ToLower().Contains(searchTermLower)) ||
                (p.Driver != null && p.Driver.FullName.ToLower().Contains(searchTermLower)) ||
                (p.Vehicle != null && (p.Vehicle.VehicleNumber.ToLower().Contains(searchTermLower) || p.Vehicle.PlateNumber.ToLower().Contains(searchTermLower))) ||
                (p.Notes != null && p.Notes.ToLower().Contains(searchTermLower)) ||
                p.Status.ToLower().Contains(searchTermLower)
            ).ToList();
        }

        if (filter.PickRequestId.HasValue)
        {
            picks = picks.Where(p => p.PickRequestId == filter.PickRequestId.Value).ToList();
        }

        if (filter.PickRequestPartId.HasValue)
        {
            picks = picks.Where(p => p.PickRequestPartId == filter.PickRequestPartId.Value).ToList();
        }

        if (filter.PartId.HasValue)
        {
            picks = picks.Where(p => p.PartId == filter.PartId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Barcode))
        {
            var barcodeLower = filter.Barcode.ToLower();
            picks = picks.Where(p => p.Barcode != null && p.Barcode.ToLower().Contains(barcodeLower)).ToList();
        }

        if (filter.PickedBy.HasValue)
        {
            picks = picks.Where(p => p.PickedBy == filter.PickedBy.Value).ToList();
        }

        if (filter.DriverId.HasValue)
        {
            picks = picks.Where(p => p.DriverId == filter.DriverId.Value).ToList();
        }

        if (filter.VehicleId.HasValue)
        {
            picks = picks.Where(p => p.VehicleId == filter.VehicleId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var statusLower = filter.Status.ToLower();
            picks = picks.Where(p => p.Status.ToLower().Contains(statusLower)).ToList();
        }

        if (filter.FromDate.HasValue)
        {
            picks = picks.Where(p => p.PickDate >= filter.FromDate.Value).ToList();
        }

        if (filter.ToDate.HasValue)
        {
            picks = picks.Where(p => p.PickDate <= filter.ToDate.Value).ToList();
        }

        picks = ApplySort(picks.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = picks.Count();

        var paginatedItems = picks
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(p => new PickDto
        {
            Id = p.Id,
            PickRequestId = p.PickRequestId,
            RequestNumber = p.PickRequest?.RequestNumber,
            PickRequestPartId = p.PickRequestPartId,
            PartId = p.PartId,
            PartCode = p.Part?.Code,
            PartName = p.Part?.Name,
            Barcode = p.Barcode,
            Quantity = p.Quantity,
            PickedBy = p.PickedBy,
            PickerName = p.Picker?.FullName,
            DriverId = p.DriverId,
            DriverName = p.Driver?.FullName,
            VehicleId = p.VehicleId,
            VehicleNumber = p.Vehicle?.VehicleNumber,
            PlateNumber = p.Vehicle?.PlateNumber,
            PickDate = p.PickDate,
            Status = p.Status,
            Notes = p.Notes,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<PickDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Pick> ApplySort(List<Domain.Entities.Pick> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "pickdate" => isDescending
                ? items.OrderByDescending(x => x.PickDate).ToList()
                : items.OrderBy(x => x.PickDate).ToList(),

            "pickrequestid" => isDescending
                ? items.OrderByDescending(x => x.PickRequestId).ToList()
                : items.OrderBy(x => x.PickRequestId).ToList(),

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

            _ => items.OrderByDescending(x => x.PickDate).ToList()
        };
    }
}
