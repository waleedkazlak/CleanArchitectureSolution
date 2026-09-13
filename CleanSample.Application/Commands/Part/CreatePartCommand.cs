using MediatR;

namespace CleanSample.Application.Commands.Part;

public class CreatePartCommand : IRequest<int>
{
    public string Code { get; set; } = null!;
    public string? Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }
    public string? Description { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }
    public string? Barcode { get; set; }
    public bool IsActive { get; set; } = true;
}
