namespace CleanSample.Application.DTOs;

/// <summary>
/// DTO for Status lookup entities
/// </summary>
public class StatusLookupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

/// <summary>
/// DTO containing all status lookups
/// </summary>
public class AllStatusesLookupDto
{
    public List<StatusLookupDto> OrderStatuses { get; set; } = new();
    public List<StatusLookupDto> LoadRequestStatuses { get; set; } = new();
    public List<StatusLookupDto> VehicleLoadStatuses { get; set; } = new();
    public List<StatusLookupDto> VehicleOffloadStatuses { get; set; } = new();
    public List<StatusLookupDto> FieldJobStatuses { get; set; } = new();
    public List<StatusLookupDto> FieldAssemblyStatuses { get; set; } = new();
    public List<StatusLookupDto> IssueStatuses { get; set; } = new();
}
