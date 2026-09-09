using MediatR;

namespace CleanSample.Application.Commands.FieldJob;

public class DeleteFieldJobCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeleteFieldJobCommand(long id)
    {
        Id = id;
    }
}
