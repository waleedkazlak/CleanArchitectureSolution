using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Design;

public class DeleteDesignCommandHandler : IRequestHandler<DeleteDesignCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDesignCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteDesignCommand request, CancellationToken cancellationToken)
    {
        var design = await _unitOfWork.Designs.GetByIdAsync(request.Id);
        if (design == null)
        {
            return false;
        }

        await _unitOfWork.Designs.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
