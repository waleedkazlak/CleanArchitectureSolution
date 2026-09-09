using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldJob;

public class GetFieldJobsWithFilterQuery : IRequest<PaginatedResultDto<FieldJobDto>>
{
    public FieldJobSearchFilterDto Filter { get; set; }

    public GetFieldJobsWithFilterQuery(FieldJobSearchFilterDto filter)
    {
        Filter = filter;
    }
}
