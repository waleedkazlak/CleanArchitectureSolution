using MediatR;

namespace CleanSample.Application.Commands.LoadRequest;

public class DeleteLoadRequestCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeleteLoadRequestCommand(long id)
    {
        Id = id;
    }
}
