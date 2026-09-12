using CleanSample.Application.DTOs.Dashboard;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Dashboard;

public class GetVehicleOperationsDashboardSummaryQueryHandler : IRequestHandler<GetVehicleOperationsDashboardSummaryQuery, VehicleOperationsDashboardSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetVehicleOperationsDashboardSummaryQueryHandler> _logger;

    public GetVehicleOperationsDashboardSummaryQueryHandler(IUnitOfWork unitOfWork, ILogger<GetVehicleOperationsDashboardSummaryQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<VehicleOperationsDashboardSummaryDto> Handle(GetVehicleOperationsDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving vehicle operations dashboard summary");

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

        return new VehicleOperationsDashboardSummaryDto
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
    }
}
