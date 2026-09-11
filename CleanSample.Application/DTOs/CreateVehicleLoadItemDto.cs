namespace CleanSample.Application.DTOs;

public class CreateVehicleLoadItemDto
{
    public long LoadRequestId { get; set; }
    public int PartId { get; set; }
    public string? Barcode { get; set; }
    public decimal Quantity { get; set; }
    public int? LoadedBy { get; set; }
    public int? DriverId { get; set; }
    public int? VehicleId { get; set; }
    public DateTime? LoadDate { get; set; }
    public string? Status { get; set; } = "Loaded";
    public string? Notes { get; set; }
}
