using MediatR;

namespace CleanSample.Application.Commands.User;

public class ToggleUserStatusCommand : IRequest<bool>
{
    public int Id { get; set; }
}
