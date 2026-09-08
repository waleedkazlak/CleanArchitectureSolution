using MediatR;

namespace CleanSample.Application.Commands.Material;

public class DeleteMaterialCommand : IRequest<bool>
{
    public int Id { get; set; }
}
