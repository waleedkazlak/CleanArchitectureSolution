using CleanSample.Application.DTOs.Dashboard;
using CleanSample.Domain.Enums;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Dashboard;

public class GetLoadRequestsDashboardSummaryQueryHandler : IRequestHandler<GetLoadRequestsDashboardSummaryQuery, LoadRequestsDashboardSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetLoadRequestsDashboardSummaryQueryHandler> _logger;

    public GetLoadRequestsDashboardSummaryQueryHandler(IUnitOfWork unitOfWork, ILogger<GetLoadRequestsDashboardSummaryQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<LoadRequestsDashboardSummaryDto> Handle(GetLoadRequestsDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving load requests dashboard summary");

        var lrStatusCounts = await _unitOfWork.LoadRequests.GetQueryable(false)
            .GroupBy(lr => lr.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var lrVerifiedCount = await _unitOfWork.LoadRequests.GetQueryable(false)
            .CountAsync(lr => lr.Verified, cancellationToken);

        var totalLoadRequests = lrStatusCounts.Sum(x => x.Count);

        return new LoadRequestsDashboardSummaryDto
        {
            TotalLoadRequests = totalLoadRequests,
            NewLoadRequests = lrStatusCounts.FirstOrDefault(x => x.Status == (int)LoadRequestStatusEnum.New)?.Count ?? 0,
            LoadingLoadRequests = lrStatusCounts.FirstOrDefault(x => x.Status == (int)LoadRequestStatusEnum.Loading)?.Count ?? 0,
            OffloadedLoadRequests = lrStatusCounts.FirstOrDefault(x => x.Status == (int)LoadRequestStatusEnum.Offloaded)?.Count ?? 0,
            CompletedLoadRequests = lrStatusCounts.FirstOrDefault(x => x.Status == (int)LoadRequestStatusEnum.Completed)?.Count ?? 0,
            CancelledLoadRequests = lrStatusCounts.FirstOrDefault(x => x.Status == (int)LoadRequestStatusEnum.Cancelled)?.Count ?? 0,
            VerifiedLoadRequests = lrVerifiedCount,
            UnverifiedLoadRequests = totalLoadRequests - lrVerifiedCount,
            StatusCounts = lrStatusCounts.ToDictionary(
                x => Enum.IsDefined(typeof(LoadRequestStatusEnum), x.Status) ? ((LoadRequestStatusEnum)x.Status).ToString() : x.Status.ToString(),
                x => x.Count)
        };
    }
}
