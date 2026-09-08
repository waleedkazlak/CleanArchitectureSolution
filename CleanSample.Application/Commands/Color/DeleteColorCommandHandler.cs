using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Color;

public class DeleteColorCommandHandler : IRequestHandler<DeleteColorCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteColorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteColorCommand request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.Colors.GetByIdAsync(request.Id);
        if (color == null)
        {
            return false;
        }

        await _unitOfWork.Colors.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
