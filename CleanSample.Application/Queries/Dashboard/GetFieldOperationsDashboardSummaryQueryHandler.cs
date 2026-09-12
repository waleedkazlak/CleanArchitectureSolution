using CleanSample.Application.DTOs.Dashboard;
using CleanSample.Domain.Enums;
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
            ScheduledJobs = jobStatusCounts.FirstOrDefault(x => x.Status == (int)FieldJobStatusEnum.Scheduled)?.Count ?? 0,
            InProgressJobs = jobStatusCounts.FirstOrDefault(x => x.Status == (int)FieldJobStatusEnum.InProgress)?.Count ?? 0,
            CompletedJobs = jobStatusCounts.FirstOrDefault(x => x.Status == (int)FieldJobStatusEnum.Completed)?.Count ?? 0,
            VerifiedJobs = verifiedJobs,
            UnverifiedJobs = totalJobs - verifiedJobs,
            TotalAssemblies = totalAssemblies,
            TotalAssembledQuantity = totalAssembledQty,
            InProgressAssemblies = assemblyStatusCounts.FirstOrDefault(x => x.Status == (int)FieldAssemblyStatusEnum.InProgress)?.Count ?? 0,
            CompletedAssemblies = assemblyStatusCounts.FirstOrDefault(x => x.Status == (int)FieldAssemblyStatusEnum.Completed)?.Count ?? 0,
            CancelledAssemblies = assemblyStatusCounts.FirstOrDefault(x => x.Status == (int)FieldAssemblyStatusEnum.Cancelled)?.Count ?? 0,
            VerifiedAssemblies = verifiedAssemblies,
            UnverifiedAssemblies = totalAssemblies - verifiedAssemblies,
            JobsByStatus = jobStatusCounts.ToDictionary(
                x => Enum.IsDefined(typeof(FieldJobStatusEnum), x.Status) ? ((FieldJobStatusEnum)x.Status).ToString() : x.Status.ToString(),
                x => x.Count),
            AssembliesByStatus = assemblyStatusCounts.ToDictionary(
                x => Enum.IsDefined(typeof(FieldAssemblyStatusEnum), x.Status) ? ((FieldAssemblyStatusEnum)x.Status).ToString() : x.Status.ToString(),
                x => x.Count)
        };
    }
}
