using MediatR;

namespace CleanSample.Application.Commands.Part;

public class CreatePartCommand : IRequest<int>
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Barcode { get; set; }
    public bool IsActive { get; set; } = true;
}
