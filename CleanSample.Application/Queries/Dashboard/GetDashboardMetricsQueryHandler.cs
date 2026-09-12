using CleanSample.Application.DTOs.Dashboard;
using CleanSample.Domain.Enums;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Dashboard;

public class GetDashboardMetricsQueryHandler : IRequestHandler<GetDashboardMetricsQuery, DashboardMetricsDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetDashboardMetricsQueryHandler> _logger;

    public GetDashboardMetricsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetDashboardMetricsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<DashboardMetricsDto> Handle(GetDashboardMetricsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving comprehensive dashboard metrics");

        // 1. Orders Summary
        var orderCountsByStatus = await _unitOfWork.Orders.GetQueryable()
            .GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var ordersSummary = new OrdersDashboardSummaryDto
        {
            TotalOrders = orderCountsByStatus.Sum(x => x.Count),
            DraftOrders = orderCountsByStatus.FirstOrDefault(x => string.Equals(x.Status, "Draft", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            PendingOrders = orderCountsByStatus.FirstOrDefault(x => string.Equals(x.Status, "Pending", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            ProcessingOrders = orderCountsByStatus.FirstOrDefault(x => string.Equals(x.Status, "Processing", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            CompletedOrders = orderCountsByStatus.FirstOrDefault(x => string.Equals(x.Status, "Completed", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            CancelledOrders = orderCountsByStatus.FirstOrDefault(x => string.Equals(x.Status, "Cancelled", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            StatusCounts = orderCountsByStatus.Where(x => !string.IsNullOrEmpty(x.Status)).ToDictionary(x => x.Status, x => x.Count)
        };

        // 2. Load Requests Summary
        var lrStatusCounts = await _unitOfWork.LoadRequests.GetQueryable(false)
            .GroupBy(lr => lr.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var lrVerifiedCount = await _unitOfWork.LoadRequests.GetQueryable(false)
            .CountAsync(lr => lr.Verified, cancellationToken);

        var totalLoadRequests = lrStatusCounts.Sum(x => x.Count);

        var loadRequestsSummary = new LoadRequestsDashboardSummaryDto
        {
            TotalLoadRequests = totalLoadRequests,
            NewLoadRequests = lrStatusCounts.FirstOrDefault(x => x.Status == LoadRequestStatus.New)?.Count ?? 0,
            LoadingLoadRequests = lrStatusCounts.FirstOrDefault(x => x.Status == LoadRequestStatus.Loading)?.Count ?? 0,
            OffloadedLoadRequests = lrStatusCounts.FirstOrDefault(x => x.Status == LoadRequestStatus.Offloaded)?.Count ?? 0,
            CompletedLoadRequests = lrStatusCounts.FirstOrDefault(x => x.Status == LoadRequestStatus.Completed)?.Count ?? 0,
            CancelledLoadRequests = lrStatusCounts.FirstOrDefault(x => x.Status == LoadRequestStatus.Cancelled)?.Count ?? 0,
            VerifiedLoadRequests = lrVerifiedCount,
            UnverifiedLoadRequests = totalLoadRequests - lrVerifiedCount,
            StatusCounts = lrStatusCounts.ToDictionary(x => x.Status.ToString(), x => x.Count)
        };

        // 3. Issues Summary
        var issueStatusCounts = await _unitOfWork.Issues.GetQueryable()
            .GroupBy(i => i.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var issueSeverityCounts = await _unitOfWork.Issues.GetQueryable()
            .GroupBy(i => i.Severity)
            .Select(g => new { Severity = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var totalIssues = issueStatusCounts.Sum(x => x.Count);

        var issuesSummary = new IssuesDashboardSummaryDto
        {
            TotalIssues = totalIssues,
            OpenIssues = issueStatusCounts.FirstOrDefault(x => string.Equals(x.Status, "Open", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            InProgressIssues = issueStatusCounts.FirstOrDefault(x => string.Equals(x.Status, "InProgress", StringComparison.OrdinalIgnoreCase) || string.Equals(x.Status, "In Progress", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            ResolvedIssues = issueStatusCounts.FirstOrDefault(x => string.Equals(x.Status, "Resolved", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            ClosedIssues = issueStatusCounts.FirstOrDefault(x => string.Equals(x.Status, "Closed", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            CriticalIssues = issueSeverityCounts.FirstOrDefault(x => string.Equals(x.Severity, "Critical", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            HighSeverityIssues = issueSeverityCounts.FirstOrDefault(x => string.Equals(x.Severity, "High", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            MediumSeverityIssues = issueSeverityCounts.FirstOrDefault(x => string.Equals(x.Severity, "Medium", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            LowSeverityIssues = issueSeverityCounts.FirstOrDefault(x => string.Equals(x.Severity, "Low", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            ByStatus = issueStatusCounts.Where(x => !string.IsNullOrEmpty(x.Status)).ToDictionary(x => x.Status, x => x.Count),
            BySeverity = issueSeverityCounts.Where(x => !string.IsNullOrEmpty(x.Severity)).ToDictionary(x => x.Severity, x => x.Count)
        };

        // 4. Vehicle Operations Summary
        var loadStatusCounts = await _unitOfWork.VehicleLoads.GetQueryable()
            .GroupBy(vl => vl.Status)
            .Select(g => new { Status = g.Key, Count = g.Count(), TotalQty = g.Sum(x => x.Quantity) })
            .ToListAsync(cancellationToken);

        var totalLoads = loadStatusCounts.Sum(x => x.Count);
        var totalLoadedQty = loadStatusCounts.Sum(x => x.TotalQty);

        var offloadStatusCounts = await _unitOfWork.VehicleOffloads.GetQueryable()
            .GroupBy(vo => vo.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var totalOffloads = offloadStatusCounts.Sum(x => x.Count);
        var verifiedOffloads = await _unitOfWork.VehicleOffloads.GetQueryable()
            .CountAsync(vo => vo.Verified, cancellationToken);

        var vehicleOpsSummary = new VehicleOperationsDashboardSummaryDto
        {
            TotalLoads = totalLoads,
            TotalLoadedQuantity = totalLoadedQty,
            TotalOffloads = totalOffloads,
            TotalOffloadedPartsCount = totalOffloads,
            VerifiedOffloads = verifiedOffloads,
            UnverifiedOffloads = totalOffloads - verifiedOffloads,
            LoadsByStatus = loadStatusCounts.Where(x => !string.IsNullOrEmpty(x.Status)).ToDictionary(x => x.Status, x => x.Count),
            OffloadsByStatus = offloadStatusCounts.Where(x => !string.IsNullOrEmpty(x.Status)).ToDictionary(x => x.Status, x => x.Count)
        };

        // 5. Field Operations Summary
        var jobStatusCounts = await _unitOfWork.FieldJobs.GetQueryable()
            .GroupBy(fj => fj.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var totalJobs = jobStatusCounts.Sum(x => x.Count);
        var verifiedJobs = await _unitOfWork.FieldJobs.GetQueryable()
            .CountAsync(fj => fj.Verified, cancellationToken);

        var assemblyStatusCounts = await _unitOfWork.FieldAssemblies.GetQueryable()
            .GroupBy(fa => fa.Status)
            .Select(g => new { Status = g.Key, Count = g.Count(), TotalQty = g.Sum(x => x.Quantity) })
            .ToListAsync(cancellationToken);

        var totalAssemblies = assemblyStatusCounts.Sum(x => x.Count);
        var totalAssembledQty = assemblyStatusCounts.Sum(x => x.TotalQty);
        var verifiedAssemblies = await _unitOfWork.FieldAssemblies.GetQueryable()
            .CountAsync(fa => fa.Verified, cancellationToken);

        var fieldOpsSummary = new FieldOperationsDashboardSummaryDto
        {
            TotalJobs = totalJobs,
            ScheduledJobs = jobStatusCounts.FirstOrDefault(x => string.Equals(x.Status, "Scheduled", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            InProgressJobs = jobStatusCounts.FirstOrDefault(x => string.Equals(x.Status, "InProgress", StringComparison.OrdinalIgnoreCase) || string.Equals(x.Status, "In Progress", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            CompletedJobs = jobStatusCounts.FirstOrDefault(x => string.Equals(x.Status, "Completed", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            VerifiedJobs = verifiedJobs,
            UnverifiedJobs = totalJobs - verifiedJobs,
            TotalAssemblies = totalAssemblies,
            TotalAssembledQuantity = totalAssembledQty,
            PendingAssemblies = assemblyStatusCounts.FirstOrDefault(x => string.Equals(x.Status, "Pending", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            CompletedAssemblies = assemblyStatusCounts.FirstOrDefault(x => string.Equals(x.Status, "Completed", StringComparison.OrdinalIgnoreCase))?.Count ?? 0,
            VerifiedAssemblies = verifiedAssemblies,
            UnverifiedAssemblies = totalAssemblies - verifiedAssemblies,
            JobsByStatus = jobStatusCounts.Where(x => !string.IsNullOrEmpty(x.Status)).ToDictionary(x => x.Status, x => x.Count),
            AssembliesByStatus = assemblyStatusCounts.Where(x => !string.IsNullOrEmpty(x.Status)).ToDictionary(x => x.Status, x => x.Count)
        };

        return new DashboardMetricsDto
        {
            GeneratedAt = DateTime.UtcNow,
            Orders = ordersSummary,
            LoadRequests = loadRequestsSummary,
            Issues = issuesSummary,
            VehicleOperations = vehicleOpsSummary,
            FieldOperations = fieldOpsSummary
        };
    }
}
