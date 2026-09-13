namespace CleanSample.Domain.Entities;

public class Color : BaseEntity
{
    public string NameEn { get; set; } = null!;
    public string? NameAr { get; set; }
    public string? Code { get; set; }

    public string Name
    {
        get => NameEn;
        set => NameEn = value;
    }
}
