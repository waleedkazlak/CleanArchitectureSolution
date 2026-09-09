namespace CleanSample.Domain.Entities;

public class ClientLocation : BaseEntity
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? City { get; set; }

    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }

    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }

    public bool IsDefault { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public ICollection<FieldJob> FieldJobs { get; set; } = new List<FieldJob>();
}
