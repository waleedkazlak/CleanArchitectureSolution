using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Color;

public class GetColorsWithFilterQueryHandler : IRequestHandler<GetColorsWithFilterQuery, PaginatedResultDto<ColorDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetColorsWithFilterQueryHandler> _logger;

    public GetColorsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetColorsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<ColorDto>> Handle(GetColorsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetColorsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var colors = await _unitOfWork.Colors.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            colors = colors.Where(c =>
                c.Name.ToLower().Contains(searchTermLower) ||
                (c.Code != null && c.Code.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        colors = ApplySort(colors.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = colors.Count();

        var paginatedColors = colors
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var colorDtos = paginatedColors.Select(c => new ColorDto
        {
            Id = c.Id,
            Name = c.Name,
            Code = c.Code,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<ColorDto>
        {
            Items = colorDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Color> ApplySort(List<Domain.Entities.Color> colors, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "name" => isDescending
                ? colors.OrderByDescending(c => c.Name).ToList()
                : colors.OrderBy(c => c.Name).ToList(),

            "code" => isDescending
                ? colors.OrderByDescending(c => c.Code).ToList()
                : colors.OrderBy(c => c.Code).ToList(),

            "createdat" => isDescending
                ? colors.OrderByDescending(c => c.CreatedAt).ToList()
                : colors.OrderBy(c => c.CreatedAt).ToList(),

            _ => colors.OrderByDescending(c => c.CreatedAt).ToList()
        };
    }
}
