namespace CleanSample.Application.DTOs;
public class CreateVehicleOffloadItemDto
{
    public long LoadRequestId { get; set; }
    public int PartId { get; set; }
    public int VehicleId { get; set; }
    public int DriverId { get; set; }
    public string? Barcode { get; set; }
    public DateTime? OffloadDate { get; set; }
    public int? Status { get; set; }
    public bool Verified { get; set; } = false;
    public int? VerifiedBy { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
}
