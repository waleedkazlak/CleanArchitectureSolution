using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Issue;

public class GetIssuesWithFilterQueryHandler : IRequestHandler<GetIssuesWithFilterQuery, PaginatedResultDto<IssueDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetIssuesWithFilterQueryHandler> _logger;

    public GetIssuesWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetIssuesWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<IssueDto>> Handle(GetIssuesWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetIssuesWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var issues = await _unitOfWork.Issues.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            issues = issues.Where(i =>
                i.IssueType.ToLower().Contains(searchTermLower) ||
                i.Description.ToLower().Contains(searchTermLower) ||
                i.Severity.ToLower().Contains(searchTermLower) ||
                (i.IssueStatus != null && i.IssueStatus.Name.ToLower().Contains(searchTermLower)) ||
                (i.ResolutionNotes != null && i.ResolutionNotes.ToLower().Contains(searchTermLower)) ||
                (i.ReportedByUser != null && i.ReportedByUser.FullName.ToLower().Contains(searchTermLower)) ||
                (i.ResolvedByUser != null && i.ResolvedByUser.FullName.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        if (filter.LoadRequestId.HasValue)
        {
            issues = issues.Where(i => i.LoadRequestId == filter.LoadRequestId.Value).ToList();
        }

        if (filter.FieldJobId.HasValue)
        {
            issues = issues.Where(i => i.FieldJobId == filter.FieldJobId.Value).ToList();
        }

        if (filter.FieldAssemblyId.HasValue)
        {
            issues = issues.Where(i => i.FieldAssemblyId == filter.FieldAssemblyId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.IssueType))
        {
            var typeLower = filter.IssueType.ToLower();
            issues = issues.Where(i => i.IssueType.ToLower().Contains(typeLower)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Severity))
        {
            var severityLower = filter.Severity.ToLower();
            issues = issues.Where(i => i.Severity.ToLower().Contains(severityLower)).ToList();
        }

        if (filter.Status.HasValue)
        {
            issues = issues.Where(i => i.Status == filter.Status.Value).ToList();
        }

        if (filter.ReportedBy.HasValue)
        {
            issues = issues.Where(i => i.ReportedBy == filter.ReportedBy.Value).ToList();
        }

        if (filter.ResolvedBy.HasValue)
        {
            issues = issues.Where(i => i.ResolvedBy == filter.ResolvedBy.Value).ToList();
        }

        if (filter.FromDate.HasValue)
        {
            issues = issues.Where(i => i.ReportedAt >= filter.FromDate.Value).ToList();
        }

        if (filter.ToDate.HasValue)
        {
            issues = issues.Where(i => i.ReportedAt <= filter.ToDate.Value).ToList();
        }

        issues = ApplySort(issues.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = issues.Count();

        var paginatedItems = issues
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(i => new IssueDto
        {
            Id = i.Id,
            LoadRequestId = i.LoadRequestId,
            FieldJobId = i.FieldJobId,
            FieldAssemblyId = i.FieldAssemblyId,
            IssueType = i.IssueType,
            Description = i.Description,
            Severity = i.Severity,
            Status = i.Status,
            StatusName = i.IssueStatus?.Name,
            ReportedBy = i.ReportedBy,
            ReportedByName = i.ReportedByUser?.FullName,
            ReportedAt = i.ReportedAt,
            ResolvedBy = i.ResolvedBy,
            ResolvedByName = i.ResolvedByUser?.FullName,
            ResolvedAt = i.ResolvedAt,
            ResolutionNotes = i.ResolutionNotes,
            CreatedAt = i.CreatedAt,
            UpdatedAt = i.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<IssueDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Issue> ApplySort(List<Domain.Entities.Issue> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "reportedat" => isDescending
                ? items.OrderByDescending(x => x.ReportedAt).ToList()
                : items.OrderBy(x => x.ReportedAt).ToList(),

            "resolvedat" => isDescending
                ? items.OrderByDescending(x => x.ResolvedAt).ToList()
                : items.OrderBy(x => x.ResolvedAt).ToList(),

            "severity" => isDescending
                ? items.OrderByDescending(x => x.Severity).ToList()
                : items.OrderBy(x => x.Severity).ToList(),

            "status" => isDescending
                ? items.OrderByDescending(x => x.Status).ToList()
                : items.OrderBy(x => x.Status).ToList(),

            "issuetype" => isDescending
                ? items.OrderByDescending(x => x.IssueType).ToList()
                : items.OrderBy(x => x.IssueType).ToList(),

            "loadrequestid" => isDescending
                ? items.OrderByDescending(x => x.LoadRequestId).ToList()
                : items.OrderBy(x => x.LoadRequestId).ToList(),

            "fieldjobid" => isDescending
                ? items.OrderByDescending(x => x.FieldJobId).ToList()
                : items.OrderBy(x => x.FieldJobId).ToList(),

            "createdat" => isDescending
                ? items.OrderByDescending(x => x.CreatedAt).ToList()
                : items.OrderBy(x => x.CreatedAt).ToList(),

            _ => items.OrderByDescending(x => x.ReportedAt).ToList()
        };
    }
}
