using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleLoad;

public class CreateVehicleLoadCommandHandler : IRequestHandler<CreateVehicleLoadCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVehicleLoadCommandHandler> _logger;

    public CreateVehicleLoadCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateVehicleLoadCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<long> Handle(CreateVehicleLoadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateVehicleLoadCommand for PickRequestId: {PickRequestId}, VehicleId: {VehicleId}",
            request.PickRequestId, request.VehicleId);

        var vehicleLoad = new Domain.Entities.VehicleLoad
        {
            PickRequestId = request.PickRequestId,
            VehicleId = request.VehicleId,
            DriverId = request.DriverId,
            LoadDate = request.LoadDate ?? DateTime.UtcNow,
            Status = string.IsNullOrWhiteSpace(request.Status) ? "Loading" : request.Status,
            Verified = request.Verified,
            VerifiedBy = request.VerifiedBy,
            VerifiedAt = request.Verified ? (request.VerifiedAt ?? DateTime.UtcNow) : null,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        if (request.VehicleLoadItems != null && request.VehicleLoadItems.Any())
        {
            foreach (var item in request.VehicleLoadItems)
            {
                var loadItem = new VehicleLoadItem
                {
                    PickId = item.PickId,
                    PartId = item.PartId,
                    Barcode = item.Barcode,
                    Quantity = item.Quantity,
                    LoadedAt = item.LoadedAt ?? DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                vehicleLoad.VehicleLoadItems.Add(loadItem);
            }
        }

        await _unitOfWork.VehicleLoads.AddAsync(vehicleLoad);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created VehicleLoad with ID: {VehicleLoadId} and {ItemCount} items",
            vehicleLoad.Id, vehicleLoad.VehicleLoadItems.Count);

        return vehicleLoad.Id;
    }
}
