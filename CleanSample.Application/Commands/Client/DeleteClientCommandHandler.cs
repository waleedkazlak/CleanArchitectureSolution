using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Client;

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(request.Id);
        if (client == null)
        {
            return false;
        }

        await _unitOfWork.Clients.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
