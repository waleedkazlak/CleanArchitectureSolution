using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.Load;

public class UpdateLoadsCommand : IRequest<List<LoadDto>>
{
    public List<UpdateLoadItemDto> Loads { get; set; } = new();

    public UpdateLoadsCommand()
    {
    }

    public UpdateLoadsCommand(List<UpdateLoadItemDto> loads)
    {
        Loads = loads;
    }
}
