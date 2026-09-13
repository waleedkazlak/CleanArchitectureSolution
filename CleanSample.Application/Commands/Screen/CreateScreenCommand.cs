using MediatR;

namespace CleanSample.Application.Commands.Screen;

public class CreateScreenCommand : IRequest<int>
{
    public string? Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Module { get; set; }
    public string? Description { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }
    public bool IsActive { get; set; } = true;
}
