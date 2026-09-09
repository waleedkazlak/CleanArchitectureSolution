using MediatR;

namespace CleanSample.Application.Commands.Role;

public class UpdateRoleCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
