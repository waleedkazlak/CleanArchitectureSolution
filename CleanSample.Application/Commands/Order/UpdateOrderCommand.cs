using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.Order;

public class UpdateOrderCommand : IRequest<bool>
{
    public long Id { get; set; }
    public int ClientId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateOnly? RequiredDate { get; set; }
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }

    public List<OrderLineItemDto> OrderLines { get; set; } = new();
}
