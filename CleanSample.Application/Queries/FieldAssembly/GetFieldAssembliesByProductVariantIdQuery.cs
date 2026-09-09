using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssembliesByProductVariantIdQuery : IRequest<List<FieldAssemblyDto>>
{
    public int ProductVariantId { get; set; }

    public GetFieldAssembliesByProductVariantIdQuery(int productVariantId)
    {
        ProductVariantId = productVariantId;
    }
}
