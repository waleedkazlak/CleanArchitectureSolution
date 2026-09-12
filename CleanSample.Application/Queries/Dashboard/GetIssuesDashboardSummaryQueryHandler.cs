using CleanSample.Application.DTOs.Dashboard;
using CleanSample.Domain.Enums;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Dashboard;

public class GetIssuesDashboardSummaryQueryHandler : IRequestHandler<GetIssuesDashboardSummaryQuery, IssuesDashboardSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetIssuesDashboardSummaryQueryHandler> _logger;

    public GetIssuesDashboardSummaryQueryHandler(IUnitOfWork unitOfWork, ILogger<GetIssuesDashboardSummaryQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IssuesDashboardSummaryDto> Handle(GetIssuesDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving issues dashboard summary");

        var issueStatusCounts = await _unitOfWork.Issues.GetQueryable()
            .GroupBy(i => i.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var issueSeverityCounts = await _unitOfWork.Issues.GetQueryable()
            .GroupBy(i => i.Severity)
            .Select(g => new { Severity = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var totalIssues = issueStatusCounts.Sum(x => x.Count);

        return new IssuesDashboardSummaryDto
        {
            TotalIssues = totalIssues,
            OpenIssues = issueStatusCounts.FirstOrDefault(x => x.Status == (int)IssueStatusEnum.Open)?.Count ?? 0,
            InProgressIssues = issueStatusCounts.FirstOrDefault(x => x.Status == (int)IssueStatusEnum.InProgress)?.Count ?? 0,
            ResolvedIssues = issueStatusCounts.FirstOrDefault(x => x.Status == (int)IssueStatusEnum.Resolved)?.Count ?? 0,
            ClosedIssues = issueStatusCounts.FirstOrDefault(x => x.Status == (int)IssueStatusEnum.Closed)?.Count ?? 0,
            CriticalIssues = issueSeverityCounts.FirstOrDefault(x => string.Equals(x.Severity, "Critical", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            HighSeverityIssues = issueSeverityCounts.FirstOrDefault(x => string.Equals(x.Severity, "High", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            MediumSeverityIssues = issueSeverityCounts.FirstOrDefault(x => string.Equals(x.Severity, "Medium", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            LowSeverityIssues = issueSeverityCounts.FirstOrDefault(x => string.Equals(x.Severity, "Low", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            ByStatus = issueStatusCounts.ToDictionary(
                x => Enum.IsDefined(typeof(IssueStatusEnum), x.Status) ? ((IssueStatusEnum)x.Status).ToString() : x.Status.ToString(),
                x => x.Count),
            BySeverity = issueSeverityCounts.Where(x => !string.IsNullOrEmpty(x.Severity)).ToDictionary(x => x.Severity, x => x.Count)
        };
    }
}
