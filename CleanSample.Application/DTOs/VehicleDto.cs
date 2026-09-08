namespace CleanSample.Application.DTOs;

public class VehicleDto
{
    public int Id { get; set; }
    public string VehicleNumber { get; set; } = null!;
    public string PlateNumber { get; set; } = null!;
    public string? VehicleType { get; set; }
    public decimal? CapacityKg { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
