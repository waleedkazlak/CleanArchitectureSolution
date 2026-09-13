using MediatR;

namespace CleanSample.Application.Commands.Role;

public class CreateRoleCommand : IRequest<int>
{
    public string? Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }
    public string? Description { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }
}
