namespace CleanSample.Application.DTOs;

public class CreateClientLocationItemDto
{
    public int? Id { get; set; }
    public int ClientId { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? City { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool IsDefault { get; set; } = false;
    public bool IsActive { get; set; } = true;
}
