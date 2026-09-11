using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssembliesByProductIdQuery : IRequest<List<FieldAssemblyDto>>
{
    public int ProductId { get; set; }

    public GetFieldAssembliesByProductIdQuery(int productId)
    {
        ProductId = productId;
    }
}
