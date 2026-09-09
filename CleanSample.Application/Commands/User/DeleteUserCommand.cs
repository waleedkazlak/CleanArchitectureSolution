using MediatR;

namespace CleanSample.Application.Commands.User;

public class DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
}
