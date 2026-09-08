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
        var color = new CleanSample.Domain.Entities.Color
        {
            Name = request.Name,
            Code = request.Code
        };

        var colorId = await _unitOfWork.Colors.AddAsync(color);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return colorId;
    }
}
