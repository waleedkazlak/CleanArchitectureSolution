using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.Order;

public class CreateOrderCommand : IRequest<long>
{
    public int ClientId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public int Status { get; set; } = (int)Domain.Enums.OrderStatusEnum.Draft;
    public string? Notes { get; set; }

    public List<OrderLineItemDto> OrderLines { get; set; } = new();
}
