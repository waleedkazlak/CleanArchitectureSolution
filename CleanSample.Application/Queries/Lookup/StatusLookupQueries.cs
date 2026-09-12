using CleanSample.Application.DTOs;
using CleanSample.Domain.Enums;
using MediatR;

namespace CleanSample.Application.Queries.Lookup;

public record GetAllStatusesLookupQuery : IRequest<AllStatusesLookupDto>;

public record GetOrderStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetLoadRequestStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetVehicleLoadStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetVehicleOffloadStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetFieldJobStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetFieldAssemblyStatusesLookupQuery : IRequest<List<StatusLookupDto>>;
public record GetIssueStatusesLookupQuery : IRequest<List<StatusLookupDto>>;

public class StatusLookupQueryHandlers :
    IRequestHandler<GetAllStatusesLookupQuery, AllStatusesLookupDto>,
    IRequestHandler<GetOrderStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetLoadRequestStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetVehicleLoadStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetVehicleOffloadStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetFieldJobStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetFieldAssemblyStatusesLookupQuery, List<StatusLookupDto>>,
    IRequestHandler<GetIssueStatusesLookupQuery, List<StatusLookupDto>>
{
    private static readonly Dictionary<OrderStatusEnum, string> OrderStatusDescriptions = new()
    {
        { OrderStatusEnum.Draft, "Order created in draft status" },
        { OrderStatusEnum.Pending, "Order pending approval or processing" },
        { OrderStatusEnum.Processing, "Order is actively being processed" },
        { OrderStatusEnum.Completed, "Order fulfilled and completed" },
        { OrderStatusEnum.Cancelled, "Order has been cancelled" }
    };

    private static readonly Dictionary<LoadRequestStatusEnum, string> LoadRequestStatusDescriptions = new()
    {
        { LoadRequestStatusEnum.New, "Load request newly created" },
        { LoadRequestStatusEnum.Loading, "Items are being loaded onto vehicle" },
        { LoadRequestStatusEnum.Offloaded, "Items have been offloaded at destination" },
        { LoadRequestStatusEnum.Completed, "Load request workflow completed" },
        { LoadRequestStatusEnum.Cancelled, "Load request has been cancelled" }
    };

    private static readonly Dictionary<VehicleLoadStatusEnum, string> VehicleLoadStatusDescriptions = new()
    {
        { VehicleLoadStatusEnum.Good, "Loaded items in good condition" },
        { VehicleLoadStatusEnum.Damaged, "Loaded items with damage" },
        { VehicleLoadStatusEnum.Missing, "Expected items missing during load" }
    };

    private static readonly Dictionary<VehicleOffloadStatusEnum, string> VehicleOffloadStatusDescriptions = new()
    {
        { VehicleOffloadStatusEnum.Good, "Offloaded items in good condition" },
        { VehicleOffloadStatusEnum.Damaged, "Offloaded items with damage" },
        { VehicleOffloadStatusEnum.Missing, "Expected items missing during offload" }
    };

    private static readonly Dictionary<FieldJobStatusEnum, string> FieldJobStatusDescriptions = new()
    {
        { FieldJobStatusEnum.Scheduled, "Field job scheduled for execution" },
        { FieldJobStatusEnum.InProgress, "Field job currently in progress" },
        { FieldJobStatusEnum.Completed, "Field job completed" },
        { FieldJobStatusEnum.Cancelled, "Field job cancelled" }
    };

    private static readonly Dictionary<FieldAssemblyStatusEnum, string> FieldAssemblyStatusDescriptions = new()
    {
        { FieldAssemblyStatusEnum.InProgress, "Assembly currently in progress" },
        { FieldAssemblyStatusEnum.Completed, "Assembly successfully completed" },
        { FieldAssemblyStatusEnum.Cancelled, "Assembly cancelled" }
    };

    private static readonly Dictionary<IssueStatusEnum, string> IssueStatusDescriptions = new()
    {
        { IssueStatusEnum.Open, "Issue is open and pending review" },
        { IssueStatusEnum.InProgress, "Issue is actively being investigated/resolved" },
        { IssueStatusEnum.Resolved, "Issue resolution has been provided" },
        { IssueStatusEnum.Closed, "Issue is verified and closed" }
    };

    public Task<AllStatusesLookupDto> Handle(GetAllStatusesLookupQuery request, CancellationToken cancellationToken)
    {
        var result = new AllStatusesLookupDto
        {
            OrderStatuses = GetOrderStatuses(),
            LoadRequestStatuses = GetLoadRequestStatuses(),
            VehicleLoadStatuses = GetVehicleLoadStatuses(),
            VehicleOffloadStatuses = GetVehicleOffloadStatuses(),
            FieldJobStatuses = GetFieldJobStatuses(),
            FieldAssemblyStatuses = GetFieldAssemblyStatuses(),
            IssueStatuses = GetIssueStatuses()
        };

        return Task.FromResult(result);
    }

    public Task<List<StatusLookupDto>> Handle(GetOrderStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetOrderStatuses());

    public Task<List<StatusLookupDto>> Handle(GetLoadRequestStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetLoadRequestStatuses());

    public Task<List<StatusLookupDto>> Handle(GetVehicleLoadStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetVehicleLoadStatuses());

    public Task<List<StatusLookupDto>> Handle(GetVehicleOffloadStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetVehicleOffloadStatuses());

    public Task<List<StatusLookupDto>> Handle(GetFieldJobStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetFieldJobStatuses());

    public Task<List<StatusLookupDto>> Handle(GetFieldAssemblyStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetFieldAssemblyStatuses());

    public Task<List<StatusLookupDto>> Handle(GetIssueStatusesLookupQuery request, CancellationToken cancellationToken)
        => Task.FromResult(GetIssueStatuses());

    private static List<StatusLookupDto> GetOrderStatuses() =>
        Enum.GetValues<OrderStatusEnum>()
            .Select(e => new StatusLookupDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = OrderStatusDescriptions.GetValueOrDefault(e, e.ToString())
            }).ToList();

    private static List<StatusLookupDto> GetLoadRequestStatuses() =>
        Enum.GetValues<LoadRequestStatusEnum>()
            .Select(e => new StatusLookupDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = LoadRequestStatusDescriptions.GetValueOrDefault(e, e.ToString())
            }).ToList();

    private static List<StatusLookupDto> GetVehicleLoadStatuses() =>
        Enum.GetValues<VehicleLoadStatusEnum>()
            .Select(e => new StatusLookupDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = VehicleLoadStatusDescriptions.GetValueOrDefault(e, e.ToString())
            }).ToList();

    private static List<StatusLookupDto> GetVehicleOffloadStatuses() =>
        Enum.GetValues<VehicleOffloadStatusEnum>()
            .Select(e => new StatusLookupDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = VehicleOffloadStatusDescriptions.GetValueOrDefault(e, e.ToString())
            }).ToList();

    private static List<StatusLookupDto> GetFieldJobStatuses() =>
        Enum.GetValues<FieldJobStatusEnum>()
            .Select(e => new StatusLookupDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = FieldJobStatusDescriptions.GetValueOrDefault(e, e.ToString())
            }).ToList();

    private static List<StatusLookupDto> GetFieldAssemblyStatuses() =>
        Enum.GetValues<FieldAssemblyStatusEnum>()
            .Select(e => new StatusLookupDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = FieldAssemblyStatusDescriptions.GetValueOrDefault(e, e.ToString())
            }).ToList();

    private static List<StatusLookupDto> GetIssueStatuses() =>
        Enum.GetValues<IssueStatusEnum>()
            .Select(e => new StatusLookupDto
            {
                Id = (int)e,
                Name = e.ToString(),
                Description = IssueStatusDescriptions.GetValueOrDefault(e, e.ToString())
            }).ToList();
}
