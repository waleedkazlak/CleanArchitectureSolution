using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Material;

public class GetMaterialsWithFilterQuery : IRequest<PaginatedResultDto<MaterialDto>>
{
    public MaterialSearchFilterDto Filter { get; set; } = new();

    public GetMaterialsWithFilterQuery()
    {
    }

    public GetMaterialsWithFilterQuery(MaterialSearchFilterDto filter)
    {
        Filter = filter ?? new MaterialSearchFilterDto();
    }
}
