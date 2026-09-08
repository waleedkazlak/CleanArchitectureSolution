using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Material;

public class GetMaterialByIdQueryHandler : IRequestHandler<GetMaterialByIdQuery, MaterialDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMaterialByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<MaterialDto?> Handle(GetMaterialByIdQuery request, CancellationToken cancellationToken)
    {
        var material = await _unitOfWork.Materials.GetByIdAsync(request.Id);
        if (material == null)
        {
            return null;
        }

        return new MaterialDto
        {
            Id = material.Id,
            Name = material.Name,
            Description = material.Description,
            CreatedAt = material.CreatedAt,
            UpdatedAt = material.UpdatedAt
        };
    }
}
