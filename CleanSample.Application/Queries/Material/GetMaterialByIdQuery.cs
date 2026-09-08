using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.Material;

public class GetMaterialByIdQuery : IRequest<MaterialDto?>
{
    public int Id { get; set; }
}
