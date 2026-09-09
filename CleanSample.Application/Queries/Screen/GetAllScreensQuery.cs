using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Screen;

public class GetAllScreensQuery : IRequest<IReadOnlyList<ScreenDto>>
{
    public bool OnlyActive { get; set; }

    public GetAllScreensQuery(bool onlyActive = false)
    {
        OnlyActive = onlyActive;
    }
}
