using MediatR;

namespace CleanSample.Application.Commands.LoadRequestLine;

public class UpdateLoadRequestLineCommand : IRequest<bool>
{
    public long Id { get; set; }
    public long LoadRequestId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
