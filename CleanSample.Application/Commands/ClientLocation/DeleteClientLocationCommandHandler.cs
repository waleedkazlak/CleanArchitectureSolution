using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.ClientLocation;

public class DeleteClientLocationCommandHandler : IRequestHandler<DeleteClientLocationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteClientLocationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteClientLocationCommand request, CancellationToken cancellationToken)
    {
        var clientLocation = await _unitOfWork.ClientLocations.GetByIdAsync(request.Id);
        if (clientLocation == null)
        {
            return false;
        }

        await _unitOfWork.ClientLocations.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
