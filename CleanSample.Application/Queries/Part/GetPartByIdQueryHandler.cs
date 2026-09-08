using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Part;

public class GetPartByIdQueryHandler : IRequestHandler<GetPartByIdQuery, PartDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPartByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PartDto?> Handle(GetPartByIdQuery request, CancellationToken cancellationToken)
    {
        var part = await _unitOfWork.Parts.GetByIdAsync(request.Id);
        if (part == null)
        {
            return null;
        }

        return new PartDto
        {
            Id = part.Id,
            Code = part.Code,
            Name = part.Name,
            Description = part.Description,
            Barcode = part.Barcode,
            IsActive = part.IsActive,
            CreatedAt = part.CreatedAt,
            UpdatedAt = part.UpdatedAt
        };
    }
}
