namespace CleanSample.Domain.Entities;

public class Vehicle : BaseEntity
{
    public string VehicleNumber { get; set; } = null!;
    public string PlateNumber { get; set; } = null!;
    public string? VehicleType { get; set; }
    public decimal? CapacityKg { get; set; }
    public bool IsActive { get; set; } = true;
}
