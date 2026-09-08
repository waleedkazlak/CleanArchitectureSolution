using MediatR;

namespace CleanSample.Application.Commands.OrderLine;

public class DeleteOrderLineCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeleteOrderLineCommand(long id)
    {
        Id = id;
    }
}
