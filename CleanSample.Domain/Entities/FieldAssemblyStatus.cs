namespace CleanSample.Domain.Entities;

/// <summary>
/// Lookup entity for Field Assembly Statuses
/// </summary>
public class FieldAssemblyStatus : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
