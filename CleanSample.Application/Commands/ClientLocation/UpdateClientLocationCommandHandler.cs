using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.ClientLocation;

public class UpdateClientLocationCommandHandler : IRequestHandler<UpdateClientLocationCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientLocationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateClientLocationCommand request, CancellationToken cancellationToken)
    {
        var clientLocation = await _unitOfWork.ClientLocations.GetByIdAsync(request.Id);
        if (clientLocation == null)
        {
            return false;
        }

        clientLocation.ClientId = request.ClientId;
        clientLocation.Name = request.Name;
        clientLocation.Address = request.Address;
        clientLocation.City = request.City;
        clientLocation.ContactName = request.ContactName;
        clientLocation.ContactPhone = request.ContactPhone;
        clientLocation.Latitude = request.Latitude;
        clientLocation.Longitude = request.Longitude;
        clientLocation.IsDefault = request.IsDefault;
        clientLocation.IsActive = request.IsActive;
        clientLocation.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.ClientLocations.UpdateAsync(clientLocation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
