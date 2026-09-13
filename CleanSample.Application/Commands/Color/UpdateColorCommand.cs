using MediatR;

namespace CleanSample.Application.Commands.Color;

public class UpdateColorCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }
    public string? Code { get; set; }
}
