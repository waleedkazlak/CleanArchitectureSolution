using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.ClientLocation;

public class GetClientLocationByIdQuery : IRequest<ClientLocationDto?>
{
    public int Id { get; set; }

    public GetClientLocationByIdQuery(int id)
    {
        Id = id;
    }
}
