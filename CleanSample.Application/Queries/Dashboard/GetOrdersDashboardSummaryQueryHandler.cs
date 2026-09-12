using CleanSample.Application.DTOs.Dashboard;
using CleanSample.Domain.Enums;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Dashboard;

public class GetOrdersDashboardSummaryQueryHandler : IRequestHandler<GetOrdersDashboardSummaryQuery, OrdersDashboardSummaryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetOrdersDashboardSummaryQueryHandler> _logger;

    public GetOrdersDashboardSummaryQueryHandler(IUnitOfWork unitOfWork, ILogger<GetOrdersDashboardSummaryQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<OrdersDashboardSummaryDto> Handle(GetOrdersDashboardSummaryQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving orders dashboard summary");

        var orderCountsByStatus = await _unitOfWork.Orders.GetQueryable()
            .GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        return new OrdersDashboardSummaryDto
        {
            TotalOrders = orderCountsByStatus.Sum(x => x.Count),
            DraftOrders = orderCountsByStatus.FirstOrDefault(x => x.Status == (int)OrderStatusEnum.Draft)?.Count ?? 0,
            PendingOrders = orderCountsByStatus.FirstOrDefault(x => x.Status == (int)OrderStatusEnum.Pending)?.Count ?? 0,
            ProcessingOrders = orderCountsByStatus.FirstOrDefault(x => x.Status == (int)OrderStatusEnum.Processing)?.Count ?? 0,
            CompletedOrders = orderCountsByStatus.FirstOrDefault(x => x.Status == (int)OrderStatusEnum.Completed)?.Count ?? 0,
            CancelledOrders = orderCountsByStatus.FirstOrDefault(x => x.Status == (int)OrderStatusEnum.Cancelled)?.Count ?? 0,
            StatusCounts = orderCountsByStatus.ToDictionary(
                x => Enum.IsDefined(typeof(OrderStatusEnum), x.Status) ? ((OrderStatusEnum)x.Status).ToString() : x.Status.ToString(),
                x => x.Count)
        };
    }
}
