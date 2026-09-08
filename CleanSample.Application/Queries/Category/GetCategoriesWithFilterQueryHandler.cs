using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Category;

public class GetCategoriesWithFilterQueryHandler : IRequestHandler<GetCategoriesWithFilterQuery, PaginatedResultDto<CategoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCategoriesWithFilterQueryHandler> _logger;

    public GetCategoriesWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCategoriesWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<CategoryDto>> Handle(GetCategoriesWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetCategoriesWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var categories = await _unitOfWork.Categories.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            categories = categories.Where(c =>
                c.Name.ToLower().Contains(searchTermLower) ||
                (c.Description != null && c.Description.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        categories = ApplySort(categories.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = categories.Count();

        var paginatedCategories = categories
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var categoryDtos = paginatedCategories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<CategoryDto>
        {
            Items = categoryDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Category> ApplySort(List<Domain.Entities.Category> categories, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "name" => isDescending
                ? categories.OrderByDescending(c => c.Name).ToList()
                : categories.OrderBy(c => c.Name).ToList(),

            "description" => isDescending
                ? categories.OrderByDescending(c => c.Description).ToList()
                : categories.OrderBy(c => c.Description).ToList(),

            "createdat" => isDescending
                ? categories.OrderByDescending(c => c.CreatedAt).ToList()
                : categories.OrderBy(c => c.CreatedAt).ToList(),

            _ => categories.OrderByDescending(c => c.CreatedAt).ToList()
        };
    }
}
