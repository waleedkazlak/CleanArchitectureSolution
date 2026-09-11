using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.ProductBOM;

public class CreateProductBOMCommand : IRequest<List<ProductBOMDto>>
{
    public int? Id { get; set; }
    public int ProductId { get; set; }
    public int PartId { get; set; }
    public decimal Quantity { get; set; }

    public List<CreateProductBOMItemDto> Items { get; set; } = new();

    public CreateProductBOMCommand()
    {
    }

    public CreateProductBOMCommand(List<CreateProductBOMItemDto> items)
    {
        Items = items ?? new List<CreateProductBOMItemDto>();
    }

    public CreateProductBOMCommand(int productId, int partId, decimal quantity, int? id = null)
    {
        ProductId = productId;
        PartId = partId;
        Quantity = quantity;
        Id = id;
    }
}
