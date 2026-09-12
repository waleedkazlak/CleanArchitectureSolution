using CleanSample.Application.DTOs;
using CleanSample.Domain.Enums;
using MediatR;

namespace CleanSample.Application.Commands.LoadRequest;

public class UpdateLoadRequestCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long? OrderId { get; set; }
    public int ClientId { get; set; }
    public int? ClientLocationId { get; set; }
    public int? RequestedBy { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public int Status { get; set; } = (int)LoadRequestStatusEnum.New;
    public string? DestinationAddress { get; set; }
    public string? DestinationCity { get; set; }
    public string? Description { get; set; }
    public int? DriverId { get; set; }
    public int? VehicleId { get; set; }
    public bool Verified { get; set; }

    public List<LoadRequestLineItemDto> LoadRequestLines { get; set; } = new();
}
