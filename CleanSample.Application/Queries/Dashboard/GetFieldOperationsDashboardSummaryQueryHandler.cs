using CleanSample.Application.DTOs.Dashboard;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Dashboard;

public class GetFieldOperationsDashboardSummaryQueryHandler : IRequestHandler<GetFieldOperationsDashboardSummaryQuery, FieldOperationsDashboardSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetFieldOperationsDashboardSummaryQueryHandler> _logger;

    public GetFieldOperationsDashboardSummaryQueryHandler(IUnitOfWork unitOfWork, ILogger<GetFieldOperationsDashboardSummaryQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<FieldOperationsDashboardSummaryDto> Handle(GetFieldOperationsDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving field operations dashboard summary");

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

        return new FieldOperationsDashboardSummaryDto
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
    }
}
