using CleanSample.Application.DTOs.Dashboard;
using MediatR;

namespace CleanSample.Application.Queries.Dashboard;

public class GetVehicleOperationsDashboardSummaryQuery : IRequest<VehicleOperationsDashboardSummaryDto>
{
}
