using MediatR;

namespace CleanSample.Application.Commands.Color;

public class CreateColorCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
}
