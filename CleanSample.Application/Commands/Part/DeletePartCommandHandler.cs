using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Part;

public class DeletePartCommandHandler : IRequestHandler<DeletePartCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeletePartCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeletePartCommand request, CancellationToken cancellationToken)
    {
        var part = await _unitOfWork.Parts.GetByIdAsync(request.Id);
        if (part == null)
        {
            return false;
        }

        await _unitOfWork.Parts.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
