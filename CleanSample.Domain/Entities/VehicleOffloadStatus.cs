namespace CleanSample.Domain.Entities;

/// <summary>
/// Lookup entity for Vehicle Offload Statuses
/// </summary>
public class VehicleOffloadStatus : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
