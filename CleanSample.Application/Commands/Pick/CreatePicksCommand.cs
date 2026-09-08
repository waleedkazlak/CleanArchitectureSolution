using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.Pick;

public class CreatePicksCommand : IRequest<List<PickDto>>
{
    public List<CreatePickItemDto> Picks { get; set; } = new();

    public CreatePicksCommand()
    {
    }

    public CreatePicksCommand(List<CreatePickItemDto> picks)
    {
        Picks = picks;
    }
}
