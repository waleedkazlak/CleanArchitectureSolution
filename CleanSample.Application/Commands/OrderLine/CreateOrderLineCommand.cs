using MediatR;

namespace CleanSample.Application.Commands.OrderLine;

public class CreateOrderLineCommand : IRequest<long>
{
    public long OrderId { get; set; }
    public int ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}
