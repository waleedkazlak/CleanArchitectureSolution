using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.Load;

public class CreateLoadsCommand : IRequest<List<LoadDto>>
{
    public List<CreateLoadItemDto> Loads { get; set; } = new();

    public CreateLoadsCommand()
    {
    }

    public CreateLoadsCommand(List<CreateLoadItemDto> loads)
    {
        Loads = loads;
    }
}
