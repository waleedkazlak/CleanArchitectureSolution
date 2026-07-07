using MediatR;

namespace CleanSample.Application.Commands.Product;
public class DeleteProductCommand : IRequest<bool>
{
    public int Id { get; set; }
}
