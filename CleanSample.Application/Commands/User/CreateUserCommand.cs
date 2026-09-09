using MediatR;

namespace CleanSample.Application.Commands.User;

public class CreateUserCommand : IRequest<int>
{
    public int? RoleId { get; set; }
    public string UserName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public bool IsActive { get; set; } = true;
}
