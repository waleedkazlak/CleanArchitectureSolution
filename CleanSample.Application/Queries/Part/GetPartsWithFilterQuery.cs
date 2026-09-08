using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Part;

public class GetPartsWithFilterQuery : IRequest<PaginatedResultDto<PartDto>>
{
    public PartSearchFilterDto Filter { get; set; }

    public GetPartsWithFilterQuery(PartSearchFilterDto filter)
    {
        Filter = filter;
    }
}
