using MediatR;

namespace CleanSample.Application.Commands.Role;

public class DeleteRoleCommand : IRequest<bool>
{
    public int Id { get; set; }
}
