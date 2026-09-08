namespace CleanSample.Domain.Entities;

public class Design : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public string? Description { get; set; }
}
