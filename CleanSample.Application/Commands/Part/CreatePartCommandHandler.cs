using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Part;

public class CreatePartCommandHandler : IRequestHandler<CreatePartCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePartCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreatePartCommand request, CancellationToken cancellationToken)
    {
        var part = new CleanSample.Domain.Entities.Part
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            Barcode = request.Barcode,
            IsActive = request.IsActive
        };

        var partId = await _unitOfWork.Parts.AddAsync(part);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return partId;
    }
}
