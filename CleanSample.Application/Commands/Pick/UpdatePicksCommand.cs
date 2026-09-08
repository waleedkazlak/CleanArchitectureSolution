using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.Pick;

public class UpdatePicksCommand : IRequest<List<PickDto>>
{
    public List<UpdatePickItemDto> Picks { get; set; } = new();

    public UpdatePicksCommand()
    {
    }

    public UpdatePicksCommand(List<UpdatePickItemDto> picks)
    {
        Picks = picks;
    }
}
