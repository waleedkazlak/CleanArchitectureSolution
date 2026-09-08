using MediatR;

namespace CleanSample.Application.Commands.Vehicle;

public class UpdateVehicleCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string VehicleNumber { get; set; } = null!;
    public string PlateNumber { get; set; } = null!;
    public string? VehicleType { get; set; }
    public decimal? CapacityKg { get; set; }
    public bool IsActive { get; set; }
}
