using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldJob;

public class GetFieldJobsByClientIdQuery : IRequest<List<FieldJobDto>>
{
    public int ClientId { get; set; }

    public GetFieldJobsByClientIdQuery(int clientId)
    {
        ClientId = clientId;
    }
}
