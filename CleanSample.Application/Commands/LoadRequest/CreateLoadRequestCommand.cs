using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.LoadRequest;

public class CreateLoadRequestCommand : IRequest<long>
{
    public string RequestNumber { get; set; } = null!;
    public long? OrderId { get; set; }
    public int ClientId { get; set; }
    public int? ClientLocationId { get; set; }
    public int? RequestedBy { get; set; }
    public DateTime? RequestDate { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public string Status { get; set; } = "Created";
    public string? DestinationAddress { get; set; }
    public string? DestinationCity { get; set; }
    public string? Description { get; set; }
    public int? DriverId { get; set; }
    public int? VehicleId { get; set; }
    public bool Verified { get; set; } = false;

    public List<LoadRequestLineItemDto> LoadRequestLines { get; set; } = new();
}
