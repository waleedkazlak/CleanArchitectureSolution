namespace CleanSample.Application.DTOs;

public class VehicleLoadItemDto
{
    public long Id { get; set; }
    public long VehicleLoadId { get; set; }
    public long? PickId { get; set; }
    public int PartId { get; set; }
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
    public string? Barcode { get; set; }
    public decimal Quantity { get; set; }
    public DateTime LoadedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
