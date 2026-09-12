using CleanSample.Application.DTOs;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleOffload;

public class CreateVehicleOffloadsCommandHandler : IRequestHandler<CreateVehicleOffloadsCommand, List<VehicleOffloadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVehicleOffloadsCommandHandler> _logger;

    public CreateVehicleOffloadsCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateVehicleOffloadsCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<VehicleOffloadDto>> Handle(CreateVehicleOffloadsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateVehicleOffloadsCommand with {Count} items", request.Offloads?.Count ?? 0);

        if (request.Offloads == null || !request.Offloads.Any())
        {
            return new List<VehicleOffloadDto>();
        }

        var newOffloads = new List<Domain.Entities.VehicleOffload>();

        foreach (var item in request.Offloads)
        {
            var offload = new Domain.Entities.VehicleOffload
            {
                LoadRequestId = item.LoadRequestId,
                PartId = item.PartId,
                VehicleId = item.VehicleId,
                DriverId = item.DriverId,
                Barcode = item.Barcode,
                OffloadDate = item.OffloadDate ?? DateTime.UtcNow,
                Status = item.Status ?? (int)Domain.Enums.VehicleOffloadStatusEnum.Good,
                Verified = item.Verified,
                VerifiedBy = item.VerifiedBy,
                VerifiedAt = item.Verified ? (item.VerifiedAt ?? DateTime.UtcNow) : null,
                Notes = item.Notes,
                CreatedAt = DateTime.UtcNow
            };

            newOffloads.Add(offload);
        }

        await _unitOfWork.VehicleOffloads.AddRangeAsync(newOffloads);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created {Count} vehicle offload records", newOffloads.Count);

        var resultDtos = new List<VehicleOffloadDto>();
        foreach (var o in newOffloads)
        {
            var fullOffload = await _unitOfWork.VehicleOffloads.GetByIdAsync(o.Id);
            if (fullOffload != null)
            {
                resultDtos.Add(new VehicleOffloadDto
                {
                    Id = fullOffload.Id,
                    LoadRequestId = fullOffload.LoadRequestId,
                    PartId = fullOffload.PartId,
                    PartCode = fullOffload.Part?.Code,
                    PartName = fullOffload.Part?.Name,
                    VehicleId = fullOffload.VehicleId,
                    VehicleNumber = fullOffload.Vehicle?.VehicleNumber,
                    PlateNumber = fullOffload.Vehicle?.PlateNumber,
                    DriverId = fullOffload.DriverId,
                    DriverName = fullOffload.Driver?.FullName,
                    Barcode = fullOffload.Barcode,
                    OffloadDate = fullOffload.OffloadDate,
                    Status = fullOffload.Status,
                    StatusName = fullOffload.VehicleOffloadStatus?.Name,
                    Verified = fullOffload.Verified,
                    VerifiedBy = fullOffload.VerifiedBy,
                    VerifierName = fullOffload.Verifier?.FullName,
                    VerifiedAt = fullOffload.VerifiedAt,
                    Notes = fullOffload.Notes,
                    CreatedAt = fullOffload.CreatedAt,
                    UpdatedAt = fullOffload.UpdatedAt
                });
            }
            else
            {
                resultDtos.Add(new VehicleOffloadDto
                {
                    Id = o.Id,
                    LoadRequestId = o.LoadRequestId,
                    PartId = o.PartId,
                    VehicleId = o.VehicleId,
                    DriverId = o.DriverId,
                    Barcode = o.Barcode,
                    OffloadDate = o.OffloadDate,
                    Status = o.Status,
                    Verified = o.Verified,
                    VerifiedBy = o.VerifiedBy,
                    VerifiedAt = o.VerifiedAt,
                    Notes = o.Notes,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt
                });
            }
        }

        return resultDtos;
    }
}
