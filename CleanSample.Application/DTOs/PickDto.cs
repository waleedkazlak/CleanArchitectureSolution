namespace CleanSample.Application.DTOs;

public class PickDto
{
    public long Id { get; set; }
    public long PickRequestId { get; set; }
    public string? RequestNumber { get; set; }
    public long? PickRequestPartId { get; set; }
    public int PartId { get; set; }
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
    public string? Barcode { get; set; }
    public decimal Quantity { get; set; }
    public int? PickedBy { get; set; }
    public string? PickerName { get; set; }
    public int? DriverId { get; set; }
    public string? DriverName { get; set; }
    public int? VehicleId { get; set; }
    public string? VehicleNumber { get; set; }
    public string? PlateNumber { get; set; }
    public DateTime PickDate { get; set; }
    public string Status { get; set; } = "Picked";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
