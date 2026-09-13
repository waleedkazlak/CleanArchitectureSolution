using MediatR;

namespace CleanSample.Application.Commands.User;

public class CreateUserCommand : IRequest<int>
{
    public int? RoleId { get; set; }
    public string UserName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? FullNameEn { get; set; }
    public string? FullNameAr { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string? Password { get; set; }
    public string PreferredLanguage { get; set; } = "en";
    public bool IsActive { get; set; } = true;
}

