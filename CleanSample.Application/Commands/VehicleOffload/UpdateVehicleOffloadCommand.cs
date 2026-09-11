using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.VehicleOffload;

public class UpdateVehicleOffloadCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long LoadRequestId { get; set; }
    public int PartId { get; set; }
    public int VehicleId { get; set; }
    public int DriverId { get; set; }
    public string? Barcode { get; set; }
    public DateTime? OffloadDate { get; set; }
    public string? Status { get; set; }
    public bool Verified { get; set; } = false;
    public int? VerifiedBy { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
}
