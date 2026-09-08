using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Design;

public class GetDesignByIdQuery : IRequest<DesignDto?>
{
    public int Id { get; set; }
}
