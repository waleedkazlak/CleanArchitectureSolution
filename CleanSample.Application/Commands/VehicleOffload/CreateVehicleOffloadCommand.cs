using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.VehicleOffload;

public class CreateVehicleOffloadCommand : IRequest<long>
{
    public long LoadRequestId { get; set; }
    public int PartId { get; set; }
    public int VehicleId { get; set; }
    public int DriverId { get; set; }
    public string? Barcode { get; set; }
    public DateTime? OffloadDate { get; set; }
    public int Status { get; set; } = (int)Domain.Enums.VehicleOffloadStatusEnum.Good;
    public bool Verified { get; set; } = false;
    public int? VerifiedBy { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
}
