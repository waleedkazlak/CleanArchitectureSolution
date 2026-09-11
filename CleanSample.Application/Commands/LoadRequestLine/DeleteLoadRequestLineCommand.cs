using MediatR;

namespace CleanSample.Application.Commands.LoadRequestLine;

public class DeleteLoadRequestLineCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeleteLoadRequestLineCommand(long id)
    {
        Id = id;
    }
}
