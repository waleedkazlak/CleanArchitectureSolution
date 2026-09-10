namespace CleanSample.Application.DTOs;

public class VehicleOffloadDto
{
    public long Id { get; set; }
    public long LoadRequestId { get; set; }
    public string? RequestNumber { get; set; }
    public int VehicleId { get; set; }
    public string? VehicleNumber { get; set; }
    public string? PlateNumber { get; set; }
    public int DriverId { get; set; }
    public string? DriverName { get; set; }
    public DateTime OffloadDate { get; set; }
    public string Status { get; set; } = "Offloading";
    public bool Verified { get; set; } = false;
    public int? VerifiedBy { get; set; }
    public string? VerifierName { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<VehicleOffloadItemDto> VehicleOffloadItems { get; set; } = new();
}
