namespace CleanSample.Application.DTOs;

public class OrderDto
{
    public long Id { get; set; }
    public int ClientId { get; set; }
    public string? ClientName { get; set; }
    public DateTime OrderDate { get; set; }
    public DateOnly? RequiredDate { get; set; }
    public string Status { get; set; } = "Draft";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<OrderLineDto> OrderLines { get; set; } = new();
}
