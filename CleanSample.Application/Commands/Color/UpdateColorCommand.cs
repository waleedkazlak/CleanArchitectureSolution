using MediatR;

namespace CleanSample.Application.Commands.Color;

public class UpdateColorCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
}
