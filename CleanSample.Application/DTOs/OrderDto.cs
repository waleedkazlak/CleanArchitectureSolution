namespace CleanSample.Application.DTOs;
public class OrderDto
{
    public long Id { get; set; }
    public int ClientId { get; set; }
    public string? ClientName { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public int Status { get; set; }
    public string? StatusName { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<OrderLineDto> OrderLines { get; set; } = new();
}
