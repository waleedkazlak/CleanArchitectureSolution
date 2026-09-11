using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.LoadRequestLine;

public class CreateLoadRequestLineCommand : IRequest<List<LoadRequestLineDto>>
{
    public long? Id { get; set; }
    public long LoadRequestId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public List<CreateLoadRequestLineItemDto> Items { get; set; } = new();

    public CreateLoadRequestLineCommand()
    {
    }

    public CreateLoadRequestLineCommand(List<CreateLoadRequestLineItemDto> items)
    {
        Items = items ?? new List<CreateLoadRequestLineItemDto>();
    }

    public CreateLoadRequestLineCommand(long loadRequestId, int productId, int quantity, long? id = null)
    {
        LoadRequestId = loadRequestId;
        ProductId = productId;
        Quantity = quantity;
        Id = id;
    }
}
