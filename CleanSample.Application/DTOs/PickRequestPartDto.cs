namespace CleanSample.Application.DTOs;

public class PickRequestPartDto
{
    public long Id { get; set; }
    public long PickRequestId { get; set; }
    public long PickRequestLineId { get; set; }
    public int PartId { get; set; }
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
    public decimal RequiredQuantity { get; set; }
    public decimal PickedQuantity { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
