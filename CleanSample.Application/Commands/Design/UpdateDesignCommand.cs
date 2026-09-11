using MediatR;

namespace CleanSample.Application.Commands.Design;

public class UpdateDesignCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
