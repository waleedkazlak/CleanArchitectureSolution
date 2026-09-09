using MediatR;

namespace CleanSample.Application.Commands.Screen;

public class DeleteScreenCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteScreenCommand(int id)
    {
        Id = id;
    }
}
