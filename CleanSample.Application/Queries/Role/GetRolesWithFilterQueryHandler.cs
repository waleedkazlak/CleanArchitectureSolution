using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Role;

public class GetRolesWithFilterQueryHandler : IRequestHandler<GetRolesWithFilterQuery, PaginatedResultDto<RoleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetRolesWithFilterQueryHandler> _logger;

    public GetRolesWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetRolesWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<RoleDto>> Handle(GetRolesWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetRolesWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var roles = await _unitOfWork.Roles.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            roles = roles.Where(r => r.Name.ToLower().Contains(searchTermLower)).ToList();
        }

        roles = ApplySort(roles.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = roles.Count();

        var paginatedRoles = roles
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var roleDtos = paginatedRoles.Select(r => new RoleDto
        {
            Id = r.Id,
            Name = r.Name,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<RoleDto>
        {
            Items = roleDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Role> ApplySort(List<Domain.Entities.Role> roles, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "name" => isDescending
                ? roles.OrderByDescending(r => r.Name).ToList()
                : roles.OrderBy(r => r.Name).ToList(),

            "createdat" => isDescending
                ? roles.OrderByDescending(r => r.CreatedAt).ToList()
                : roles.OrderBy(r => r.CreatedAt).ToList(),

            _ => roles.OrderByDescending(r => r.CreatedAt).ToList()
        };
    }
}
