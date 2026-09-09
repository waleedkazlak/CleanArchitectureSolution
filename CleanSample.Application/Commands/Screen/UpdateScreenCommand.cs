using MediatR;

namespace CleanSample.Application.Commands.Screen;

public class UpdateScreenCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Module { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}
