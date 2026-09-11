namespace CleanSample.Application.DTOs;

public class LoadRequestLineDto
{
    public long Id { get; set; }
    public long LoadRequestId { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<LoadRequestPartDto> LoadRequestParts { get; set; } = new();
}
