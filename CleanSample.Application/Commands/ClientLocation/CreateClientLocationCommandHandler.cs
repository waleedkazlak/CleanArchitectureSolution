using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.ClientLocation;

public class CreateClientLocationCommandHandler : IRequestHandler<CreateClientLocationCommand, List<ClientLocationDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateClientLocationCommandHandler> _logger;

    public CreateClientLocationCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateClientLocationCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<ClientLocationDto>> Handle(CreateClientLocationCommand request, CancellationToken cancellationToken)
    {
        var itemsToProcess = new List<CreateClientLocationItemDto>();

        if (request.Items != null && request.Items.Any())
        {
            itemsToProcess.AddRange(request.Items);
        }
        else if (request.ClientId > 0 && !string.IsNullOrWhiteSpace(request.Name))
        {
            itemsToProcess.Add(new CreateClientLocationItemDto
            {
                Id = request.Id,
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
            });
        }

        if (!itemsToProcess.Any())
        {
            _logger.LogWarning("CreateClientLocationCommand called with no items to process");
            return new List<ClientLocationDto>();
        }

        _logger.LogInformation("Processing {Count} ClientLocation items (create/update)", itemsToProcess.Count);

        var processedIds = new List<int>();

        foreach (var item in itemsToProcess)
        {
            Domain.Entities.ClientLocation? existing = null;

            // 1. If an explicit Id > 0 is provided, find by Id
            if (item.Id.HasValue && item.Id.Value > 0)
            {
                existing = await _unitOfWork.ClientLocations.GetByIdAsync(item.Id.Value);
            }

            // 2. If not found by Id, check if a record with the same (ClientId, Name) already exists
            if (existing == null)
            {
                existing = await _unitOfWork.ClientLocations.GetByNameAndClientIdAsync(item.ClientId, item.Name);
            }

            if (existing != null)
            {
                // Update existing record
                existing.ClientId = item.ClientId;
                existing.Name = item.Name;
                existing.Address = item.Address;
                existing.City = item.City;
                existing.ContactName = item.ContactName;
                existing.ContactPhone = item.ContactPhone;
                existing.Latitude = item.Latitude;
                existing.Longitude = item.Longitude;
                existing.IsDefault = item.IsDefault;
                existing.IsActive = item.IsActive;
                existing.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.ClientLocations.UpdateAsync(existing);
                processedIds.Add(existing.Id);
                _logger.LogInformation("Updated existing ClientLocation with Id: {Id}", existing.Id);
            }
            else
            {
                // Create new record
                var newLocation = new Domain.Entities.ClientLocation
                {
                    ClientId = item.ClientId,
                    Name = item.Name,
                    Address = item.Address,
                    City = item.City,
                    ContactName = item.ContactName,
                    ContactPhone = item.ContactPhone,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    IsDefault = item.IsDefault,
                    IsActive = item.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                var newId = await _unitOfWork.ClientLocations.AddAsync(newLocation);
                processedIds.Add(newId);
                _logger.LogInformation("Created new ClientLocation with Id: {Id}", newId);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Fetch full entity details with relations to map to DTOs
        var resultDtos = new List<ClientLocationDto>();
        foreach (var id in processedIds)
        {
            var fullLocation = await _unitOfWork.ClientLocations.GetByIdAsync(id);
            if (fullLocation != null)
            {
                resultDtos.Add(new ClientLocationDto
                {
                    Id = fullLocation.Id,
                    ClientId = fullLocation.ClientId,
                    ClientName = fullLocation.Client?.Name,
                    Name = fullLocation.Name,
                    Address = fullLocation.Address,
                    City = fullLocation.City,
                    ContactName = fullLocation.ContactName,
                    ContactPhone = fullLocation.ContactPhone,
                    Latitude = fullLocation.Latitude,
                    Longitude = fullLocation.Longitude,
                    IsDefault = fullLocation.IsDefault,
                    IsActive = fullLocation.IsActive,
                    CreatedAt = fullLocation.CreatedAt,
                    UpdatedAt = fullLocation.UpdatedAt
                });
            }
        }

        _logger.LogInformation("Successfully processed {Count} ClientLocation items", resultDtos.Count);
        return resultDtos;
    }
}
