using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Design;

public class UpdateDesignCommandHandler : IRequestHandler<UpdateDesignCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDesignCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateDesignCommand request, CancellationToken cancellationToken)
    {
        var design = await _unitOfWork.Designs.GetByIdAsync(request.Id);
        if (design == null)
        {
            return false;
        }

        design.Name = request.Name;
        design.Description = request.Description;
        design.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Designs.UpdateAsync(design);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
