using MediatR;

namespace CleanSample.Application.Commands.Design;

public class CreateDesignCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public string? Description { get; set; }
}
