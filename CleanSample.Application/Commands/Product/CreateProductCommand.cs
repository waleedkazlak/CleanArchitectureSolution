using MediatR;

namespace CleanSample.Application.Commands.Product;
public class CreateProductCommand : IRequest<int>
{
    public int CategoryId { get; set; }
    public int? ColorId { get; set; }
    public int? MaterialId { get; set; }
    public int? DesignId { get; set; }
    public string? Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }
    public string? Barcode { get; set; }
    public string? Description { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }
    public string? PictureUrl { get; set; }
    public bool IsActive { get; set; } = true;
}
