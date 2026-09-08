using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Color;

public class GetColorsWithFilterQuery : IRequest<PaginatedResultDto<ColorDto>>
{
    public ColorSearchFilterDto Filter { get; set; } = new();

    public GetColorsWithFilterQuery()
    {
    }

    public GetColorsWithFilterQuery(ColorSearchFilterDto filter)
    {
        Filter = filter ?? new ColorSearchFilterDto();
    }
}
