using MediatR;

namespace CleanSample.Application.Commands.Color;

public class DeleteColorCommand : IRequest<bool>
{
    public int Id { get; set; }
}
