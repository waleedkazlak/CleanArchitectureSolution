using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Screen;

public class GetScreensWithFilterQuery : IRequest<PaginatedResultDto<ScreenDto>>
{
    public ScreenSearchFilterDto Filter { get; set; } = new();

    public GetScreensWithFilterQuery()
    {
    }

    public GetScreensWithFilterQuery(ScreenSearchFilterDto filter)
    {
        Filter = filter ?? new ScreenSearchFilterDto();
    }
}

