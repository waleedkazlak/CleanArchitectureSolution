using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.ClientLocation;

public class GetClientLocationsWithFilterQueryHandler : IRequestHandler<GetClientLocationsWithFilterQuery, PaginatedResultDto<ClientLocationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetClientLocationsWithFilterQueryHandler> _logger;

    public GetClientLocationsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetClientLocationsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<ClientLocationDto>> Handle(GetClientLocationsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetClientLocationsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var locations = await _unitOfWork.ClientLocations.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            locations = locations.Where(l =>
                (l.Client != null && l.Client.Name.ToLower().Contains(searchTermLower)) ||
                l.Name.ToLower().Contains(searchTermLower) ||
                l.Address.ToLower().Contains(searchTermLower) ||
                (l.City != null && l.City.ToLower().Contains(searchTermLower)) ||
                (l.ContactName != null && l.ContactName.ToLower().Contains(searchTermLower)) ||
                (l.ContactPhone != null && l.ContactPhone.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        if (filter.ClientId.HasValue)
        {
            locations = locations.Where(l => l.ClientId == filter.ClientId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var nameLower = filter.Name.ToLower();
            locations = locations.Where(l => l.Name.ToLower().Contains(nameLower)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            var cityLower = filter.City.ToLower();
            locations = locations.Where(l => l.City != null && l.City.ToLower().Contains(cityLower)).ToList();
        }

        if (filter.IsDefault.HasValue)
        {
            locations = locations.Where(l => l.IsDefault == filter.IsDefault.Value).ToList();
        }

        if (filter.IsActive.HasValue)
        {
            locations = locations.Where(l => l.IsActive == filter.IsActive.Value).ToList();
        }

        locations = ApplySort(locations.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = locations.Count();

        var paginatedItems = locations
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(l => new ClientLocationDto
        {
            Id = l.Id,
            ClientId = l.ClientId,
            ClientName = l.Client?.Name,
            Name = l.Name,
            Address = l.Address,
            City = l.City,
            ContactName = l.ContactName,
            ContactPhone = l.ContactPhone,
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            IsDefault = l.IsDefault,
            IsActive = l.IsActive,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<ClientLocationDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.ClientLocation> ApplySort(List<Domain.Entities.ClientLocation> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "name" => isDescending
                ? items.OrderByDescending(x => x.Name).ToList()
                : items.OrderBy(x => x.Name).ToList(),

            "clientid" => isDescending
                ? items.OrderByDescending(x => x.ClientId).ToList()
                : items.OrderBy(x => x.ClientId).ToList(),

            "city" => isDescending
                ? items.OrderByDescending(x => x.City).ToList()
                : items.OrderBy(x => x.City).ToList(),

            "createdat" => isDescending
                ? items.OrderByDescending(x => x.CreatedAt).ToList()
                : items.OrderBy(x => x.CreatedAt).ToList(),

            _ => items.OrderByDescending(x => x.CreatedAt).ToList()
        };
    }
}
