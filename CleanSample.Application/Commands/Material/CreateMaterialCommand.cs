using MediatR;

namespace CleanSample.Application.Commands.Material;

public class CreateMaterialCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
