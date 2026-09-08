using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Client;

public class GetClientByIdQuery : IRequest<ClientDto?>
{
    public int Id { get; set; }

    public GetClientByIdQuery(int id)
    {
        Id = id;
    }
}
