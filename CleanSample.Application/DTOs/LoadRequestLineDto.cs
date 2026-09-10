namespace CleanSample.Application.DTOs;

public class LoadRequestLineDto
{
    public long Id { get; set; }
    public long LoadRequestId { get; set; }
    public string? RequestNumber { get; set; }
    public int ProductVariantId { get; set; }
    public string? ProductVariantCode { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<LoadRequestPartDto> LoadRequestParts { get; set; } = new();
}
