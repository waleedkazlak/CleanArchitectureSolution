using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Screen;

public class GetScreensWithFilterQueryHandler : IRequestHandler<GetScreensWithFilterQuery, PaginatedResultDto<ScreenDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetScreensWithFilterQueryHandler> _logger;

    public GetScreensWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetScreensWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<ScreenDto>> Handle(GetScreensWithFilterQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting screens with filter");

        var screens = await _unitOfWork.Screens.GetAllAsync(cancellationToken);
        var query = screens.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Filter.SearchTerm))
        {
            var search = request.Filter.SearchTerm.Trim().ToLowerInvariant();
            query = query.Where(s => s.Name.ToLowerInvariant().Contains(search) ||
                                     s.Code.ToLowerInvariant().Contains(search) ||
                                     (s.Module != null && s.Module.ToLowerInvariant().Contains(search)));
        }

        if (!string.IsNullOrWhiteSpace(request.Filter.Code))
        {
            query = query.Where(s => s.Code.Equals(request.Filter.Code.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(request.Filter.Module))
        {
            query = query.Where(s => s.Module != null && s.Module.Equals(request.Filter.Module.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        if (request.Filter.IsActive.HasValue)
        {
            query = query.Where(s => s.IsActive == request.Filter.IsActive.Value);
        }

        var totalCount = query.Count();

        // Sorting
        query = request.Filter.SortBy?.ToLowerInvariant() switch
        {
            "name" => request.Filter.SortDirection?.ToLowerInvariant() == "desc" ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
            "code" => request.Filter.SortDirection?.ToLowerInvariant() == "desc" ? query.OrderByDescending(s => s.Code) : query.OrderBy(s => s.Code),
            "module" => request.Filter.SortDirection?.ToLowerInvariant() == "desc" ? query.OrderByDescending(s => s.Module) : query.OrderBy(s => s.Module),
            "createdat" => request.Filter.SortDirection?.ToLowerInvariant() == "desc" ? query.OrderByDescending(s => s.CreatedAt) : query.OrderBy(s => s.CreatedAt),
            _ => query.OrderBy(s => s.Module).ThenBy(s => s.Name)
        };

        var pageNumber = request.Filter.PageNumber > 0 ? request.Filter.PageNumber : 1;
        var pageSize = request.Filter.PageSize > 0 ? request.Filter.PageSize : 10;

        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new ScreenDto
            {
                ScreenId = s.Id,
                Name = s.Name,
                Code = s.Code,
                Module = s.Module,
                Description = s.Description,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            })
            .ToList();

        return new PaginatedResultDto<ScreenDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}


