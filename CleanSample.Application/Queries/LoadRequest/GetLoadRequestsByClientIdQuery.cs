using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequest;

public class GetLoadRequestsByClientIdQuery : IRequest<IEnumerable<LoadRequestDto>>
{
    public int ClientId { get; set; }

    public GetLoadRequestsByClientIdQuery(int clientId)
    {
        ClientId = clientId;
    }
}
