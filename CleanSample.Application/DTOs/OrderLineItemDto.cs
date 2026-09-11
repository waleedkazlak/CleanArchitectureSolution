namespace CleanSample.Application.DTOs;

public class OrderLineItemDto
{
    public long Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}
