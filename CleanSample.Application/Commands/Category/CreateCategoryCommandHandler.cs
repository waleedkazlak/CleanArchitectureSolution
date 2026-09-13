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
        var nameEn = !string.IsNullOrWhiteSpace(request.NameEn) ? request.NameEn : (request.Name ?? string.Empty);
        var nameAr = request.NameAr;
        var descEn = !string.IsNullOrWhiteSpace(request.DescriptionEn) ? request.DescriptionEn : request.Description;
        var descAr = request.DescriptionAr;

        var category = new CleanSample.Domain.Entities.Category
        {
            NameEn = nameEn,
            NameAr = nameAr,
            DescriptionEn = descEn,
            DescriptionAr = descAr
        };

        var categoryId = await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return categoryId;
    }
}
