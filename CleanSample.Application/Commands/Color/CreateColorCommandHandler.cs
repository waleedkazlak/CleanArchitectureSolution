using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Color;

public class CreateColorCommandHandler : IRequestHandler<CreateColorCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateColorCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateColorCommand request, CancellationToken cancellationToken)
    {
        var nameEn = !string.IsNullOrWhiteSpace(request.NameEn) ? request.NameEn : (request.Name ?? string.Empty);
        var nameAr = request.NameAr;

        var color = new CleanSample.Domain.Entities.Color
        {
            NameEn = nameEn,
            NameAr = nameAr,
            Code = request.Code
        };

        var colorId = await _unitOfWork.Colors.AddAsync(color);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return colorId;
    }
}
