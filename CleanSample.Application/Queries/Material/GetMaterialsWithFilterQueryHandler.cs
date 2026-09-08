using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Material;

public class GetMaterialsWithFilterQueryHandler : IRequestHandler<GetMaterialsWithFilterQuery, PaginatedResultDto<MaterialDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetMaterialsWithFilterQueryHandler> _logger;

    public GetMaterialsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetMaterialsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<MaterialDto>> Handle(GetMaterialsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetMaterialsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var materials = await _unitOfWork.Materials.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            materials = materials.Where(m =>
                m.Name.ToLower().Contains(searchTermLower) ||
                (m.Description != null && m.Description.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        materials = ApplySort(materials.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = materials.Count();

        var paginatedMaterials = materials
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var materialDtos = paginatedMaterials.Select(m => new MaterialDto
        {
            Id = m.Id,
            Name = m.Name,
            Description = m.Description,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<MaterialDto>
        {
            Items = materialDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Material> ApplySort(List<Domain.Entities.Material> materials, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "name" => isDescending
                ? materials.OrderByDescending(m => m.Name).ToList()
                : materials.OrderBy(m => m.Name).ToList(),

            "description" => isDescending
                ? materials.OrderByDescending(m => m.Description).ToList()
                : materials.OrderBy(m => m.Description).ToList(),

            "createdat" => isDescending
                ? materials.OrderByDescending(m => m.CreatedAt).ToList()
                : materials.OrderBy(m => m.CreatedAt).ToList(),

            _ => materials.OrderByDescending(m => m.CreatedAt).ToList()
        };
    }
}
