using MediatR;

namespace CleanSample.Application.Commands.Category;

public class DeleteCategoryCommand : IRequest<bool>
{
    public int Id { get; set; }
}
