using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.OrderLine;

public class CreateOrderLineCommand : IRequest<List<OrderLineDto>>
{
    public long? Id { get; set; }
    public long OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }

    public List<CreateOrderLineItemDto> Items { get; set; } = new();

    public CreateOrderLineCommand()
    {
    }

    public CreateOrderLineCommand(List<CreateOrderLineItemDto> items)
    {
        Items = items ?? new List<CreateOrderLineItemDto>();
    }

    public CreateOrderLineCommand(long orderId, int productId, int quantity, string? notes = null, long? id = null)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Notes = notes;
        Id = id;
    }
}
