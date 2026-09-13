using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Design;

public class GetDesignsWithFilterQueryHandler : IRequestHandler<GetDesignsWithFilterQuery, PaginatedResultDto<DesignDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetDesignsWithFilterQueryHandler> _logger;

    public GetDesignsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetDesignsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<DesignDto>> Handle(GetDesignsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetDesignsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var designs = await _unitOfWork.Designs.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            designs = designs.Where(d =>
                d.NameEn.ToLower().Contains(searchTermLower) ||
                (d.NameAr != null && d.NameAr.ToLower().Contains(searchTermLower)) ||
                (d.DescriptionEn != null && d.DescriptionEn.ToLower().Contains(searchTermLower)) ||
                (d.DescriptionAr != null && d.DescriptionAr.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        designs = ApplySort(designs.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = designs.Count();

        var paginatedDesigns = designs
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var designDtos = paginatedDesigns.Select(d => new DesignDto
        {
            Id = d.Id,
            Name = CleanSample.Application.Helpers.LocalizationHelper.Localize(d.NameEn, d.NameAr) ?? d.NameEn,
            NameEn = d.NameEn,
            NameAr = d.NameAr,
            Description = CleanSample.Application.Helpers.LocalizationHelper.Localize(d.DescriptionEn, d.DescriptionAr),
            DescriptionEn = d.DescriptionEn,
            DescriptionAr = d.DescriptionAr,
            CreatedAt = d.CreatedAt,
            UpdatedAt = d.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<DesignDto>
        {
            Items = designDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Design> ApplySort(List<Domain.Entities.Design> designs, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "name" => isDescending
                ? designs.OrderByDescending(d => d.Name).ToList()
                : designs.OrderBy(d => d.Name).ToList(),

            "description" => isDescending
                ? designs.OrderByDescending(d => d.Description).ToList()
                : designs.OrderBy(d => d.Description).ToList(),

            "createdat" => isDescending
                ? designs.OrderByDescending(d => d.CreatedAt).ToList()
                : designs.OrderBy(d => d.CreatedAt).ToList(),

            _ => designs.OrderByDescending(d => d.CreatedAt).ToList()
        };
    }
}
