using MediatR;

namespace CleanSample.Application.Commands.Category;

public class CreateCategoryCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
