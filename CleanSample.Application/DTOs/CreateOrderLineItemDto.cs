namespace CleanSample.Application.DTOs;

public class CreateOrderLineItemDto
{
    public long? Id { get; set; }
    public long OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}
