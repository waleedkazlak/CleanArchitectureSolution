using MediatR;

namespace CleanSample.Application.Commands.Part;

public class DeletePartCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeletePartCommand(int id)
    {
        Id = id;
    }
}
