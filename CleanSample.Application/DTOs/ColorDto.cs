namespace CleanSample.Application.DTOs;

public class ColorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string NameEn { get; set; } = string.Empty;
    public string? NameAr { get; set; }
    public string? Code { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
