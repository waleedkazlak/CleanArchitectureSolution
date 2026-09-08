using MediatR;

namespace CleanSample.Application.Commands.Design;

public class DeleteDesignCommand : IRequest<bool>
{
    public int Id { get; set; }
}
