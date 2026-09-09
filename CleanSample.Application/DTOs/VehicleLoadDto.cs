namespace CleanSample.Application.DTOs;

public class VehicleLoadDto
{
    public long Id { get; set; }
    public long PickRequestId { get; set; }
    public string? RequestNumber { get; set; }
    public int VehicleId { get; set; }
    public string? VehicleNumber { get; set; }
    public string? PlateNumber { get; set; }
    public int DriverId { get; set; }
    public string? DriverName { get; set; }
    public DateTime LoadDate { get; set; }
    public string Status { get; set; } = "Loading";
    public bool Verified { get; set; } = false;
    public int? VerifiedBy { get; set; }
    public string? VerifierName { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<VehicleLoadItemDto> VehicleLoadItems { get; set; } = new();
}
