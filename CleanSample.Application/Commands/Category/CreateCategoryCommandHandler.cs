using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Category;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new CleanSample.Domain.Entities.Category
        {
            Name = request.Name,
            Description = request.Description
        };

        var categoryId = await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return categoryId;
    }
}
