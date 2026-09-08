using MediatR;

namespace CleanSample.Application.Commands.Order;

public class CreateOrderCommand : IRequest<long>
{
    public string OrderNumber { get; set; } = null!;
    public int ClientId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateOnly? RequiredDate { get; set; }
    public string Status { get; set; } = "Draft";
    public string? Notes { get; set; }
}
