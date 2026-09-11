namespace CleanSample.Domain.Entities;

public class Order : BaseEntity<long>
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateOnly? RequiredDate { get; set; }
    public string Status { get; set; } = "Draft";
    public string? Notes { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}
