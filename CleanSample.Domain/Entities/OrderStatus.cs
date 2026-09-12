namespace CleanSample.Domain.Entities;

/// <summary>
/// Lookup entity for Order Statuses
/// </summary>
public class OrderStatus : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
