namespace CleanSample.Domain.Entities;

public class Client : BaseEntity
{
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<FieldJob> FieldJobs { get; set; } = new List<FieldJob>();
}
