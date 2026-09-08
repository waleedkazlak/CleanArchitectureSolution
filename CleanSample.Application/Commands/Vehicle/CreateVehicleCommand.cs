using MediatR;

namespace CleanSample.Application.Commands.Vehicle;

public class CreateVehicleCommand : IRequest<int>
{
    public string VehicleNumber { get; set; } = null!;
    public string PlateNumber { get; set; } = null!;
    public string? VehicleType { get; set; }
    public decimal? CapacityKg { get; set; }
}
