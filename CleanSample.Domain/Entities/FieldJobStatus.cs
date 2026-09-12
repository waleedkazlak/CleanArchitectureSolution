namespace CleanSample.Domain.Entities;

/// <summary>
/// Lookup entity for Field Job Statuses
/// </summary>
public class FieldJobStatus : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
