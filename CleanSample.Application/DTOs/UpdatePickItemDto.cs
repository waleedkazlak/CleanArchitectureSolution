namespace CleanSample.Application.DTOs;

public class UpdatePickItemDto
{
    public long Id { get; set; }
    public long PickRequestId { get; set; }
    public long? PickRequestPartId { get; set; }
    public int PartId { get; set; }
    public string? Barcode { get; set; }
    public decimal Quantity { get; set; }
    public int? PickedBy { get; set; }
    public int? DriverId { get; set; }
    public int? VehicleId { get; set; }
    public DateTime? PickDate { get; set; }
    public string? Status { get; set; }
    public string? Notes { get; set; }
}
