using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.LoadRequest;

public class DeleteLoadRequestCommandHandler : IRequestHandler<DeleteLoadRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteLoadRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteLoadRequestCommand request, CancellationToken cancellationToken)
    {
        var loadRequest = await _unitOfWork.LoadRequests.GetByIdAsync(request.Id);
        if (loadRequest == null)
        {
            return false;
        }

        await _unitOfWork.LoadRequests.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
