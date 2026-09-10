namespace CleanSample.Domain.Entities;

public class LoadRequest : BaseEntity<long>
{
    public string RequestNumber { get; set; } = null!;

    public long? OrderId { get; set; }
    public Order? Order { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int? ClientLocationId { get; set; }
    public ClientLocation? ClientLocation { get; set; }

    public int? RequestedBy { get; set; }
    public User? Requester { get; set; }

    public DateTime RequestDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExecutionDate { get; set; }

    public string Status { get; set; } = "Created";

    public string? DestinationAddress { get; set; }
    public string? DestinationCity { get; set; }

    public string? Description { get; set; }

    public int? DriverId { get; set; }
    public User? Driver { get; set; }

    public int? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public bool Verified { get; set; } = false;

    public ICollection<LoadRequestLine> LoadRequestLines { get; set; } = new List<LoadRequestLine>();
    public ICollection<LoadRequestPart> LoadRequestParts { get; set; } = new List<LoadRequestPart>();
    public ICollection<Load> Loads { get; set; } = new List<Load>();
    public ICollection<VehicleOffload> VehicleOffloads { get; set; } = new List<VehicleOffload>();
    public ICollection<FieldJob> FieldJobs { get; set; } = new List<FieldJob>();
    public ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
