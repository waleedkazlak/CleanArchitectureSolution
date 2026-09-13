using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.User;

public class GetUsersWithFilterQueryHandler : IRequestHandler<GetUsersWithFilterQuery, PaginatedResultDto<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUsersWithFilterQueryHandler> _logger;

    public GetUsersWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUsersWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<UserDto>> Handle(GetUsersWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetUsersWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}, RoleId: {RoleId}, IsActive: {IsActive}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm, filter.RoleId, filter.IsActive);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var users = await _unitOfWork.Users.GetAllAsync();

        if (filter.RoleId.HasValue)
        {
            users = users.Where(u => u.RoleId == filter.RoleId.Value);
        }

        if (filter.IsActive.HasValue)
        {
            users = users.Where(u => u.IsActive == filter.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.PreferredLanguage))
        {
            var lang = filter.PreferredLanguage.Trim().ToLower();
            users = users.Where(u => u.PreferredLanguage.ToLower() == lang);
        }

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            users = users.Where(u =>
                u.UserName.ToLower().Contains(searchTermLower) ||
                u.FullNameEn.ToLower().Contains(searchTermLower) ||
                (u.FullNameAr != null && u.FullNameAr.ToLower().Contains(searchTermLower)) ||
                (u.Email != null && u.Email.ToLower().Contains(searchTermLower)) ||
                (u.Mobile != null && u.Mobile.ToLower().Contains(searchTermLower)) ||
                (u.Role != null && (u.Role.NameEn.ToLower().Contains(searchTermLower) || (u.Role.NameAr != null && u.Role.NameAr.ToLower().Contains(searchTermLower))))
            );
        }

        var sortedUsers = ApplySort(users.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = sortedUsers.Count;

        var paginatedUsers = sortedUsers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var userDtos = paginatedUsers.Select(u => new UserDto
        {
            Id = u.Id,
            RoleId = u.RoleId,
            RoleName = CleanSample.Application.Helpers.LocalizationHelper.Localize(u.Role?.NameEn, u.Role?.NameAr),
            UserName = u.UserName,
            FullName = CleanSample.Application.Helpers.LocalizationHelper.Localize(u.FullNameEn, u.FullNameAr) ?? u.FullNameEn,
            FullNameEn = u.FullNameEn,
            FullNameAr = u.FullNameAr,
            Email = u.Email,
            Mobile = u.Mobile,
            PreferredLanguage = u.PreferredLanguage,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt,
            UpdatedAt = u.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<UserDto>
        {
            Items = userDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.User> ApplySort(List<Domain.Entities.User> users, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "username" => isDescending
                ? users.OrderByDescending(u => u.UserName).ToList()
                : users.OrderBy(u => u.UserName).ToList(),

            "fullname" => isDescending
                ? users.OrderByDescending(u => u.FullName).ToList()
                : users.OrderBy(u => u.FullName).ToList(),

            "email" => isDescending
                ? users.OrderByDescending(u => u.Email).ToList()
                : users.OrderBy(u => u.Email).ToList(),

            "isactive" => isDescending
                ? users.OrderByDescending(u => u.IsActive).ToList()
                : users.OrderBy(u => u.IsActive).ToList(),

            "role" => isDescending
                ? users.OrderByDescending(u => u.Role?.Name).ToList()
                : users.OrderBy(u => u.Role?.Name).ToList(),

            "createdat" => isDescending
                ? users.OrderByDescending(u => u.CreatedAt).ToList()
                : users.OrderBy(u => u.CreatedAt).ToList(),

            _ => users.OrderByDescending(u => u.CreatedAt).ToList()
        };
    }
}
