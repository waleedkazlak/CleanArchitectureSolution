using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Category;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category == null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(request.NameEn))
            category.NameEn = request.NameEn;
        else if (!string.IsNullOrWhiteSpace(request.Name))
            category.NameEn = request.Name;

        if (request.NameAr != null)
            category.NameAr = request.NameAr;

        if (!string.IsNullOrWhiteSpace(request.DescriptionEn))
            category.DescriptionEn = request.DescriptionEn;
        else if (request.Description != null)
            category.DescriptionEn = request.Description;

        if (request.DescriptionAr != null)
            category.DescriptionAr = request.DescriptionAr;

        category.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
