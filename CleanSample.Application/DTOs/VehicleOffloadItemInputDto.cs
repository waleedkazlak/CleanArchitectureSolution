namespace CleanSample.Application.DTOs;

public class VehicleOffloadItemInputDto
{
    public long? Id { get; set; }
    public long? LoadId { get; set; }
    public int PartId { get; set; }
    public string? Barcode { get; set; }
    public decimal Quantity { get; set; }
    public DateTime? OffloadedAt { get; set; }
}
