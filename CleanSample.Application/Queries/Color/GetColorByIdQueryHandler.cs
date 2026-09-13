using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Color;

public class GetColorByIdQueryHandler : IRequestHandler<GetColorByIdQuery, ColorDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetColorByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ColorDto?> Handle(GetColorByIdQuery request, CancellationToken cancellationToken)
    {
        var color = await _unitOfWork.Colors.GetByIdAsync(request.Id);
        if (color == null)
        {
            return null;
        }

        return new ColorDto
        {
            Id = color.Id,
            Name = CleanSample.Application.Helpers.LocalizationHelper.Localize(color.NameEn, color.NameAr) ?? color.NameEn,
            NameEn = color.NameEn,
            NameAr = color.NameAr,
            Code = color.Code,
            CreatedAt = color.CreatedAt,
            UpdatedAt = color.UpdatedAt
        };
    }
}
