using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.ClientLocation;

public class GetClientLocationByIdQueryHandler : IRequestHandler<GetClientLocationByIdQuery, ClientLocationDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetClientLocationByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ClientLocationDto?> Handle(GetClientLocationByIdQuery request, CancellationToken cancellationToken)
    {
        var cl = await _unitOfWork.ClientLocations.GetByIdAsync(request.Id);
        if (cl == null)
        {
            return null;
        }

        return new ClientLocationDto
        {
            Id = cl.Id,
            ClientId = cl.ClientId,
            ClientName = cl.Client?.Name,
            Name = cl.Name,
            Address = cl.Address,
            City = cl.City,
            ContactName = cl.ContactName,
            ContactPhone = cl.ContactPhone,
            Latitude = cl.Latitude,
            Longitude = cl.Longitude,
            IsDefault = cl.IsDefault,
            IsActive = cl.IsActive,
            CreatedAt = cl.CreatedAt,
            UpdatedAt = cl.UpdatedAt
        };
    }
}
