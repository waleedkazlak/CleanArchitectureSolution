using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.ClientLocation;

public class GetClientLocationsByClientIdQueryHandler : IRequestHandler<GetClientLocationsByClientIdQuery, IEnumerable<ClientLocationDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetClientLocationsByClientIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ClientLocationDto>> Handle(GetClientLocationsByClientIdQuery request, CancellationToken cancellationToken)
    {
        var locations = await _unitOfWork.ClientLocations.GetByClientIdAsync(request.ClientId);

        return locations.Select(l => new ClientLocationDto
        {
            Id = l.Id,
            ClientId = l.ClientId,
            ClientName = l.Client?.Name,
            Name = l.Name,
            Address = l.Address,
            City = l.City,
            ContactName = l.ContactName,
            ContactPhone = l.ContactPhone,
            Latitude = l.Latitude,
            Longitude = l.Longitude,
            IsDefault = l.IsDefault,
            IsActive = l.IsActive,
            CreatedAt = l.CreatedAt,
            UpdatedAt = l.UpdatedAt
        }).ToList();
    }
}
