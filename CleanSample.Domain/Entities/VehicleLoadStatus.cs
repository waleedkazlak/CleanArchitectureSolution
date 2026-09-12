namespace CleanSample.Domain.Entities;

/// <summary>
/// Lookup entity for Vehicle Load Statuses
/// </summary>
public class VehicleLoadStatus : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
