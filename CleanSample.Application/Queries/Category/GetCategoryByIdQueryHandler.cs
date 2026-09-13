using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Category;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category == null)
        {
            return null;
        }

        return new CategoryDto
        {
            Id = category.Id,
            Name = CleanSample.Application.Helpers.LocalizationHelper.Localize(category.NameEn, category.NameAr) ?? category.NameEn,
            NameEn = category.NameEn,
            NameAr = category.NameAr,
            Description = CleanSample.Application.Helpers.LocalizationHelper.Localize(category.DescriptionEn, category.DescriptionAr),
            DescriptionEn = category.DescriptionEn,
            DescriptionAr = category.DescriptionAr,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}
