using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Screen;

public class GetScreenByIdQuery : IRequest<ScreenDto?>
{
    public int Id { get; set; }

    public GetScreenByIdQuery(int id)
    {
        Id = id;
    }
}
