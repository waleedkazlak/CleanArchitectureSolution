using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.ClientLocation;

public class CreateClientLocationCommandHandler : IRequestHandler<CreateClientLocationCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientLocationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateClientLocationCommand request, CancellationToken cancellationToken)
    {
        var clientLocation = new CleanSample.Domain.Entities.ClientLocation
        {
            ClientId = request.ClientId,
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            ContactName = request.ContactName,
            ContactPhone = request.ContactPhone,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            IsDefault = request.IsDefault,
            IsActive = request.IsActive
        };

        var clientLocationId = await _unitOfWork.ClientLocations.AddAsync(clientLocation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return clientLocationId;
    }
}
