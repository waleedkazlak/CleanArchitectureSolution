namespace CleanSample.Domain.Entities;

/// <summary>
/// Lookup entity for Issue Statuses
/// </summary>
public class IssueStatus : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
