using MediatR;

namespace CleanSample.Application.Commands.ClientLocation;

public class DeleteClientLocationCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteClientLocationCommand(int id)
    {
        Id = id;
    }
}
