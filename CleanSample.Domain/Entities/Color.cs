namespace CleanSample.Domain.Entities;

public class Color : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
}
