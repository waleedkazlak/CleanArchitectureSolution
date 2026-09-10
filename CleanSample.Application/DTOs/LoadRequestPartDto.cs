namespace CleanSample.Application.DTOs;

public class LoadRequestPartDto
{
    public long Id { get; set; }
    public long LoadRequestId { get; set; }
    public long LoadRequestLineId { get; set; }
    public int PartId { get; set; }
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
    public decimal RequiredQuantity { get; set; }
    public decimal LoadedQuantity { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
