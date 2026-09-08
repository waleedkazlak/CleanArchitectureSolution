using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.ClientLocation;

public class GetClientLocationsByClientIdQuery : IRequest<IEnumerable<ClientLocationDto>>
{
    public int ClientId { get; set; }

    public GetClientLocationsByClientIdQuery(int clientId)
    {
        ClientId = clientId;
    }
}
