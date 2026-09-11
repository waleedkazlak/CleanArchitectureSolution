using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Design;

public class CreateDesignCommandHandler : IRequestHandler<CreateDesignCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDesignCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateDesignCommand request, CancellationToken cancellationToken)
    {
        var design = new CleanSample.Domain.Entities.Design
        {
            Name = request.Name,
            Description = request.Description
        };

        var designId = await _unitOfWork.Designs.AddAsync(design);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return designId;
    }
}
