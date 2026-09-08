using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Client;

public class GetClientsWithFilterQuery : IRequest<PaginatedResultDto<ClientDto>>
{
    public ClientSearchFilterDto Filter { get; set; }

    public GetClientsWithFilterQuery(ClientSearchFilterDto filter)
    {
        Filter = filter;
    }
}
