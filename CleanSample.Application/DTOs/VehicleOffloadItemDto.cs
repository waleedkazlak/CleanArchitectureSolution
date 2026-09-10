namespace CleanSample.Application.DTOs;

public class VehicleOffloadItemDto
{
    public long Id { get; set; }
    public long VehicleOffloadId { get; set; }
    public long? LoadId { get; set; }
    public int PartId { get; set; }
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
    public string? Barcode { get; set; }
    public decimal Quantity { get; set; }
    public DateTime OffloadedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
