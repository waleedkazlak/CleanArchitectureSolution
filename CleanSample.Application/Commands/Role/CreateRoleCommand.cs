using MediatR;

namespace CleanSample.Application.Commands.Role;

public class CreateRoleCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
}
