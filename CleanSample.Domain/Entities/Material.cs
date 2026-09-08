namespace CleanSample.Domain.Entities;

public class Material : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
