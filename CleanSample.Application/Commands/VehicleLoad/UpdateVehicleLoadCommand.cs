using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.VehicleLoad;

public class UpdateVehicleLoadCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long PickRequestId { get; set; }
    public int VehicleId { get; set; }
    public int DriverId { get; set; }
    public DateTime? LoadDate { get; set; }
    public string Status { get; set; } = "Loading";
    public bool Verified { get; set; } = false;
    public int? VerifiedBy { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }

    public List<VehicleLoadItemInputDto> VehicleLoadItems { get; set; } = new();
}
