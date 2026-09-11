namespace CleanSample.Application.DTOs;

public class CreateLoadRequestLineItemDto
{
    public long? Id { get; set; }
    public long LoadRequestId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
