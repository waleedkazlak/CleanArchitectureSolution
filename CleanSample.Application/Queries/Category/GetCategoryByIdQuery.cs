using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Category;

public class GetCategoryByIdQuery : IRequest<CategoryDto?>
{
    public int Id { get; set; }
}
