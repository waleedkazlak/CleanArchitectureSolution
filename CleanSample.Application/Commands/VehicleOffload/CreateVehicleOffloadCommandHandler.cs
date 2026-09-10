using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleOffload;

public class CreateVehicleOffloadCommandHandler : IRequestHandler<CreateVehicleOffloadCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVehicleOffloadCommandHandler> _logger;

    public CreateVehicleOffloadCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateVehicleOffloadCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<long> Handle(CreateVehicleOffloadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateVehicleOffloadCommand for LoadRequestId: {LoadRequestId}, VehicleId: {VehicleId}",
            request.LoadRequestId, request.VehicleId);

        var vehicleOffload = new Domain.Entities.VehicleOffload
        {
            LoadRequestId = request.LoadRequestId,
            VehicleId = request.VehicleId,
            DriverId = request.DriverId,
            OffloadDate = request.OffloadDate ?? DateTime.UtcNow,
            Status = string.IsNullOrWhiteSpace(request.Status) ? "Offloading" : request.Status,
            Verified = request.Verified,
            VerifiedBy = request.VerifiedBy,
            VerifiedAt = request.Verified ? (request.VerifiedAt ?? DateTime.UtcNow) : null,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        if (request.VehicleOffloadItems != null && request.VehicleOffloadItems.Any())
        {
            foreach (var item in request.VehicleOffloadItems)
            {
                var offloadItem = new VehicleOffloadItem
                {
                    LoadId = item.LoadId,
                    PartId = item.PartId,
                    Barcode = item.Barcode,
                    Quantity = item.Quantity,
                    OffloadedAt = item.OffloadedAt ?? DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                vehicleOffload.VehicleOffloadItems.Add(offloadItem);
            }
        }

        await _unitOfWork.VehicleOffloads.AddAsync(vehicleOffload);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created VehicleOffload with ID: {VehicleOffloadId} and {ItemCount} items",
            vehicleOffload.Id, vehicleOffload.VehicleOffloadItems.Count);

        return vehicleOffload.Id;
    }
}
