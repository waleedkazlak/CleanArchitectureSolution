namespace CleanSample.Application.DTOs;
public class VehicleLoadDto
{
    public long Id { get; set; }
    public long LoadRequestId { get; set; }
    public int PartId { get; set; }
    public string? PartCode { get; set; }
    public string? PartName { get; set; }
    public string? Barcode { get; set; }
    public decimal Quantity { get; set; }
    public int? LoadedBy { get; set; }
    public string? LoaderName { get; set; }
    public int? DriverId { get; set; }
    public string? DriverName { get; set; }
    public int? VehicleId { get; set; }
    public string? VehicleNumber { get; set; }
    public string? PlateNumber { get; set; }
    public DateTime LoadDate { get; set; }
    public int Status { get; set; }
    public string? StatusName { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
