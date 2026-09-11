using MediatR;

namespace CleanSample.Application.Commands.Client;

public class CreateClientCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public bool IsActive { get; set; } = true;
}
