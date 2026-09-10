using CleanSample.Application.DTOs;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Load;

public class CreateLoadsCommandHandler : IRequestHandler<CreateLoadsCommand, List<LoadDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateLoadsCommandHandler> _logger;

    public CreateLoadsCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateLoadsCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<LoadDto>> Handle(CreateLoadsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateLoadsCommand with {Count} items", request.Loads?.Count ?? 0);

        if (request.Loads == null || !request.Loads.Any())
        {
            return new List<LoadDto>();
        }

        var newLoads = new List<Domain.Entities.Load>();

        foreach (var item in request.Loads)
        {
            long? loadRequestPartId = item.LoadRequestPartId;

            // If LoadRequestPartId is not specified, attempt to resolve from LoadRequestParts matching LoadRequestId & PartId
            if (!loadRequestPartId.HasValue)
            {
                var relatedParts = await _unitOfWork.LoadRequestParts.GetByLoadRequestIdAsync(item.LoadRequestId);
                var matchingPart = relatedParts.FirstOrDefault(p => p.PartId == item.PartId);
                if (matchingPart != null)
                {
                    loadRequestPartId = matchingPart.Id;
                }
            }

            var load = new Domain.Entities.Load
            {
                LoadRequestId = item.LoadRequestId,
                LoadRequestPartId = loadRequestPartId,
                PartId = item.PartId,
                Barcode = item.Barcode,
                Quantity = item.Quantity,
                LoadedBy = item.LoadedBy,
                DriverId = item.DriverId,
                VehicleId = item.VehicleId,
                LoadDate = item.LoadDate ?? DateTime.UtcNow,
                Status = string.IsNullOrWhiteSpace(item.Status) ? "Loaded" : item.Status,
                Notes = item.Notes,
                CreatedAt = DateTime.UtcNow
            };

            newLoads.Add(load);

            // Update LoadRequestPart LoadedQuantity and Status if linked
            if (loadRequestPartId.HasValue)
            {
                var lrp = await _unitOfWork.LoadRequestParts.GetByIdAsync(loadRequestPartId.Value);
                if (lrp != null)
                {
                    lrp.LoadedQuantity += item.Quantity;
                    if (lrp.LoadedQuantity >= lrp.RequiredQuantity)
                    {
                        lrp.Status = "Completed";
                    }
                    else if (lrp.LoadedQuantity > 0)
                    {
                        lrp.Status = "PartiallyLoaded";
                    }
                    await _unitOfWork.LoadRequestParts.UpdateAsync(lrp);
                }
            }
        }

        await _unitOfWork.Loads.AddRangeAsync(newLoads);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created {Count} load records", newLoads.Count);

        // Map to DTOs
        var resultDtos = new List<LoadDto>();
        foreach (var l in newLoads)
        {
            var fullLoad = await _unitOfWork.Loads.GetByIdAsync(l.Id);
            if (fullLoad != null)
            {
                resultDtos.Add(new LoadDto
                {
                    Id = fullLoad.Id,
                    LoadRequestId = fullLoad.LoadRequestId,
                    RequestNumber = fullLoad.LoadRequest?.RequestNumber,
                    LoadRequestPartId = fullLoad.LoadRequestPartId,
                    PartId = fullLoad.PartId,
                    PartCode = fullLoad.Part?.Code,
                    PartName = fullLoad.Part?.Name,
                    Barcode = fullLoad.Barcode,
                    Quantity = fullLoad.Quantity,
                    LoadedBy = fullLoad.LoadedBy,
                    LoaderName = fullLoad.Loader?.FullName,
                    DriverId = fullLoad.DriverId,
                    DriverName = fullLoad.Driver?.FullName,
                    VehicleId = fullLoad.VehicleId,
                    VehicleNumber = fullLoad.Vehicle?.VehicleNumber,
                    PlateNumber = fullLoad.Vehicle?.PlateNumber,
                    LoadDate = fullLoad.LoadDate,
                    Status = fullLoad.Status,
                    Notes = fullLoad.Notes,
                    CreatedAt = fullLoad.CreatedAt,
                    UpdatedAt = fullLoad.UpdatedAt
                });
            }
            else
            {
                resultDtos.Add(new LoadDto
                {
                    Id = l.Id,
                    LoadRequestId = l.LoadRequestId,
                    LoadRequestPartId = l.LoadRequestPartId,
                    PartId = l.PartId,
                    Barcode = l.Barcode,
                    Quantity = l.Quantity,
                    LoadedBy = l.LoadedBy,
                    DriverId = l.DriverId,
                    VehicleId = l.VehicleId,
                    LoadDate = l.LoadDate,
                    Status = l.Status,
                    Notes = l.Notes,
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt
                });
            }
        }

        return resultDtos;
    }
}
