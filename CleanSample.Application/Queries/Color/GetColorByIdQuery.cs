using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Color;

public class GetColorByIdQuery : IRequest<ColorDto?>
{
    public int Id { get; set; }
}
