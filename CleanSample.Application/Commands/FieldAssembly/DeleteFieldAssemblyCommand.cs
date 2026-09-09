using MediatR;

namespace CleanSample.Application.Commands.FieldAssembly;

public class DeleteFieldAssemblyCommand : IRequest<bool>
{
    public long Id { get; set; }

    public DeleteFieldAssemblyCommand(long id)
    {
        Id = id;
    }
}
