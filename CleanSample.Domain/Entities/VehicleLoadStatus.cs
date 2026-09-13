namespace CleanSample.Domain.Entities;

/// <summary>
/// Lookup entity for Vehicle Load Statuses
/// </summary>
public class VehicleLoadStatus : BaseEntity
{
    public string NameEn { get; set; } = null!;
    public string? NameAr { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }

    public string Name
    {
        get => NameEn;
        set => NameEn = value;
    }
    public string? Description
    {
        get => DescriptionEn;
        set => DescriptionEn = value;
    }
}
