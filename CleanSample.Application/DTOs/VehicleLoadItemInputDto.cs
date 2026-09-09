namespace CleanSample.Application.DTOs;

public class VehicleLoadItemInputDto
{
    public long? Id { get; set; }
    public long? PickId { get; set; }
    public int PartId { get; set; }
    public string? Barcode { get; set; }
    public decimal Quantity { get; set; }
    public DateTime? LoadedAt { get; set; }
}
