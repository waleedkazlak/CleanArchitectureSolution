namespace CleanSample.Domain.Entities;

public class Part : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public bool IsActive { get; set; } = true;
}
