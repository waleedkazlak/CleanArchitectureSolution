using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Design;

public class GetDesignByIdQueryHandler : IRequestHandler<GetDesignByIdQuery, DesignDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDesignByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<DesignDto?> Handle(GetDesignByIdQuery request, CancellationToken cancellationToken)
    {
        var design = await _unitOfWork.Designs.GetByIdAsync(request.Id);
        if (design == null)
        {
            return null;
        }

        return new DesignDto
        {
            Id = design.Id,
            Name = design.Name,
            Description = design.Description,
            CreatedAt = design.CreatedAt,
            UpdatedAt = design.UpdatedAt
        };
    }
}
