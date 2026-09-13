using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Part;

public class GetPartsWithFilterQueryHandler : IRequestHandler<GetPartsWithFilterQuery, PaginatedResultDto<PartDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPartsWithFilterQueryHandler> _logger;

    public GetPartsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPartsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<PartDto>> Handle(GetPartsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetPartsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var parts = await _unitOfWork.Parts.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            parts = parts.Where(p =>
                p.Code.ToLower().Contains(searchTermLower) ||
                p.NameEn.ToLower().Contains(searchTermLower) ||
                (p.NameAr != null && p.NameAr.ToLower().Contains(searchTermLower)) ||
                (p.Barcode != null && p.Barcode.ToLower().Contains(searchTermLower)) ||
                (p.DescriptionEn != null && p.DescriptionEn.ToLower().Contains(searchTermLower)) ||
                (p.DescriptionAr != null && p.DescriptionAr.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Code))
        {
            var codeLower = filter.Code.ToLower();
            parts = parts.Where(p => p.Code.ToLower().Contains(codeLower)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var nameLower = filter.Name.ToLower();
            parts = parts.Where(p => 
                p.NameEn.ToLower().Contains(nameLower) ||
                (p.NameAr != null && p.NameAr.ToLower().Contains(nameLower))
            ).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Barcode))
        {
            var barcodeLower = filter.Barcode.ToLower();
            parts = parts.Where(p => p.Barcode != null && p.Barcode.ToLower().Contains(barcodeLower)).ToList();
        }

        if (filter.IsActive.HasValue)
        {
            parts = parts.Where(p => p.IsActive == filter.IsActive.Value).ToList();
        }

        parts = ApplySort(parts.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = parts.Count();

        var paginatedParts = parts
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var partDtos = paginatedParts.Select(p => new PartDto
        {
            Id = p.Id,
            Code = p.Code,
            Name = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.NameEn, p.NameAr) ?? p.NameEn,
            NameEn = p.NameEn,
            NameAr = p.NameAr,
            Description = CleanSample.Application.Helpers.LocalizationHelper.Localize(p.DescriptionEn, p.DescriptionAr),
            DescriptionEn = p.DescriptionEn,
            DescriptionAr = p.DescriptionAr,
            Barcode = p.Barcode,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<PartDto>
        {
            Items = partDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Part> ApplySort(List<Domain.Entities.Part> parts, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "code" => isDescending
                ? parts.OrderByDescending(p => p.Code).ToList()
                : parts.OrderBy(p => p.Code).ToList(),

            "name" => isDescending
                ? parts.OrderByDescending(p => p.Name).ToList()
                : parts.OrderBy(p => p.Name).ToList(),

            "barcode" => isDescending
                ? parts.OrderByDescending(p => p.Barcode).ToList()
                : parts.OrderBy(p => p.Barcode).ToList(),

            "createdat" => isDescending
                ? parts.OrderByDescending(p => p.CreatedAt).ToList()
                : parts.OrderBy(p => p.CreatedAt).ToList(),

            _ => parts.OrderByDescending(p => p.CreatedAt).ToList()
        };
    }
}
