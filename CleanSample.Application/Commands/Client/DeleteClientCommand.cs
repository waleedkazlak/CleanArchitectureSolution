using MediatR;

namespace CleanSample.Application.Commands.Client;

public class DeleteClientCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteClientCommand(int id)
    {
        Id = id;
    }
}
