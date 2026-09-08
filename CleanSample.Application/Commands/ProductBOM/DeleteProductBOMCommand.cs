using MediatR;

namespace CleanSample.Application.Commands.ProductBOM;

public class DeleteProductBOMCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteProductBOMCommand(int id)
    {
        Id = id;
    }
}
