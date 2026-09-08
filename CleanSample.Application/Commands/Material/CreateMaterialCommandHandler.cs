using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Material;

public class CreateMaterialCommandHandler : IRequestHandler<CreateMaterialCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMaterialCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = new CleanSample.Domain.Entities.Material
        {
            Name = request.Name,
            Description = request.Description
        };

        var materialId = await _unitOfWork.Materials.AddAsync(material);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return materialId;
    }
}
