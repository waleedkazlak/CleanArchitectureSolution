using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.PickRequest;

public class DeletePickRequestCommandHandler : IRequestHandler<DeletePickRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePickRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeletePickRequestCommand request, CancellationToken cancellationToken)
    {
        var pickRequest = await _unitOfWork.PickRequests.GetByIdAsync(request.Id);
        if (pickRequest == null)
        {
            return false;
        }

        await _unitOfWork.PickRequests.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
