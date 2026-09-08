using MediatR;

namespace CleanSample.Application.Commands.OrderLine;

public class UpdateOrderLineCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public int ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
}
