namespace CleanSample.Application.DTOs;
public class LoadRequestDto
{
    public long Id { get; set; }
    public long? OrderId { get; set; }
    public int ClientId { get; set; }
    public string? ClientName { get; set; }
    public int? ClientLocationId { get; set; }
    public string? ClientLocationName { get; set; }
    public int? RequestedBy { get; set; }
    public string? RequesterName { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public int Status { get; set; }
    public string? StatusName { get; set; }
    public string? DestinationAddress { get; set; }
    public string? DestinationCity { get; set; }
    public string? Description { get; set; }
    public int? DriverId { get; set; }
    public string? DriverName { get; set; }
    public int? VehicleId { get; set; }
    public string? VehicleNumber { get; set; }
    public string? PlateNumber { get; set; }
    public bool Verified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<LoadRequestLineDto> LoadRequestLines { get; set; } = new();
    public List<LoadRequestPartDto> LoadRequestParts { get; set; } = new();
}
