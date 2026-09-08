using MediatR;

namespace CleanSample.Application.Commands.PickRequest;

public class DeletePickRequestCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeletePickRequestCommand(long id)
    {
        Id = id;
    }
}
