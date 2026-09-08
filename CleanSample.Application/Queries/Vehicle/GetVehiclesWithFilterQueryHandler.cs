using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Vehicle;

public class GetVehiclesWithFilterQueryHandler : IRequestHandler<GetVehiclesWithFilterQuery, PaginatedResultDto<VehicleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetVehiclesWithFilterQueryHandler> _logger;

    public GetVehiclesWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetVehiclesWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<VehicleDto>> Handle(GetVehiclesWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetVehiclesWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var vehicles = await _unitOfWork.Vehicles.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            vehicles = vehicles.Where(v =>
                v.VehicleNumber.ToLower().Contains(searchTermLower) ||
                v.PlateNumber.ToLower().Contains(searchTermLower) ||
                (v.VehicleType != null && v.VehicleType.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        if (filter.MinCapacityKg.HasValue)
        {
            vehicles = vehicles.Where(v => v.CapacityKg >= filter.MinCapacityKg.Value).ToList();
        }

        if (filter.MaxCapacityKg.HasValue)
        {
            vehicles = vehicles.Where(v => v.CapacityKg <= filter.MaxCapacityKg.Value).ToList();
        }

        if (filter.IsActive.HasValue)
        {
            vehicles = vehicles.Where(v => v.IsActive == filter.IsActive.Value).ToList();
        }

        vehicles = ApplySort(vehicles.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = vehicles.Count();

        var paginatedVehicles = vehicles
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var vehicleDtos = paginatedVehicles.Select(v => new VehicleDto
        {
            Id = v.Id,
            VehicleNumber = v.VehicleNumber,
            PlateNumber = v.PlateNumber,
            VehicleType = v.VehicleType,
            CapacityKg = v.CapacityKg,
            IsActive = v.IsActive,
            CreatedAt = v.CreatedAt,
            UpdatedAt = v.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<VehicleDto>
        {
            Items = vehicleDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Vehicle> ApplySort(List<Domain.Entities.Vehicle> vehicles, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "vehiclenumber" => isDescending
                ? vehicles.OrderByDescending(v => v.VehicleNumber).ToList()
                : vehicles.OrderBy(v => v.VehicleNumber).ToList(),

            "platenumber" => isDescending
                ? vehicles.OrderByDescending(v => v.PlateNumber).ToList()
                : vehicles.OrderBy(v => v.PlateNumber).ToList(),

            "capacitykg" => isDescending
                ? vehicles.OrderByDescending(v => v.CapacityKg).ToList()
                : vehicles.OrderBy(v => v.CapacityKg).ToList(),

            "createdat" => isDescending
                ? vehicles.OrderByDescending(v => v.CreatedAt).ToList()
                : vehicles.OrderBy(v => v.CreatedAt).ToList(),

            _ => vehicles.OrderByDescending(v => v.CreatedAt).ToList()
        };
    }
}
