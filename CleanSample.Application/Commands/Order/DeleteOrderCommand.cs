using MediatR;

namespace CleanSample.Application.Commands.Order;

public class DeleteOrderCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeleteOrderCommand(long id)
    {
        Id = id;
    }
}
