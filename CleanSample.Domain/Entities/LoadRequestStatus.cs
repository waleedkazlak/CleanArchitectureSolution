namespace CleanSample.Domain.Entities;

/// <summary>
/// Lookup entity for Load Request Statuses
/// </summary>
public class LoadRequestStatus : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
