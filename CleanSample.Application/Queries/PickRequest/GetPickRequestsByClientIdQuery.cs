using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.PickRequest;

public class GetPickRequestsByClientIdQuery : IRequest<IEnumerable<PickRequestDto>>
{
    public int ClientId { get; set; }

    public GetPickRequestsByClientIdQuery(int clientId)
    {
        ClientId = clientId;
    }
}
