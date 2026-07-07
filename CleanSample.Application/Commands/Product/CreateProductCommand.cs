using MediatR;

namespace CleanSample.Application.Commands.Product;
public class CreateProductCommand : IRequest<int>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
