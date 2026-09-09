using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssembliesWithFilterQuery : IRequest<PaginatedResultDto<FieldAssemblyDto>>
{
    public FieldAssemblySearchFilterDto Filter { get; set; }

    public GetFieldAssembliesWithFilterQuery(FieldAssemblySearchFilterDto filter)
    {
        Filter = filter;
    }
}
