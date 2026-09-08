using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Category;

public class GetCategoriesWithFilterQuery : IRequest<PaginatedResultDto<CategoryDto>>
{
    public CategorySearchFilterDto Filter { get; set; } = new();

    public GetCategoriesWithFilterQuery()
    {
    }

    public GetCategoriesWithFilterQuery(CategorySearchFilterDto filter)
    {
        Filter = filter ?? new CategorySearchFilterDto();
    }
}
