using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Pick;

public class UpdatePicksCommandHandler : IRequestHandler<UpdatePicksCommand, List<PickDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePicksCommandHandler> _logger;

    public UpdatePicksCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdatePicksCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<PickDto>> Handle(UpdatePicksCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdatePicksCommand with {Count} items", request.Picks?.Count ?? 0);

        if (request.Picks == null || !request.Picks.Any())
        {
            return new List<PickDto>();
        }

        var updatedPicks = new List<Domain.Entities.Pick>();

        foreach (var item in request.Picks)
        {
            var existingPick = await _unitOfWork.Picks.GetByIdAsync(item.Id);
            if (existingPick == null)
            {
                _logger.LogWarning("Pick with id {PickId} not found for update", item.Id);
                continue;
            }

            var oldQuantity = existingPick.Quantity;
            var oldPartId = existingPick.PickRequestPartId;

            existingPick.PickRequestId = item.PickRequestId;
            existingPick.PickRequestPartId = item.PickRequestPartId;
            existingPick.PartId = item.PartId;
            existingPick.Barcode = item.Barcode;
            existingPick.Quantity = item.Quantity;
            existingPick.PickedBy = item.PickedBy;
            existingPick.DriverId = item.DriverId;
            existingPick.VehicleId = item.VehicleId;
            if (item.PickDate.HasValue)
            {
                existingPick.PickDate = item.PickDate.Value;
            }
            if (!string.IsNullOrWhiteSpace(item.Status))
            {
                existingPick.Status = item.Status;
            }
            existingPick.Notes = item.Notes;
            existingPick.UpdatedAt = DateTime.UtcNow;

            // Adjust PickRequestPart PickedQuantity
            if (oldPartId.HasValue && oldPartId == item.PickRequestPartId)
            {
                var prp = await _unitOfWork.PickRequestParts.GetByIdAsync(oldPartId.Value);
                if (prp != null)
                {
                    prp.PickedQuantity = Math.Max(0, prp.PickedQuantity - oldQuantity + item.Quantity);
                    if (prp.PickedQuantity >= prp.RequiredQuantity)
                    {
                        prp.Status = "Completed";
                    }
                    else if (prp.PickedQuantity > 0)
                    {
                        prp.Status = "PartiallyPicked";
                    }
                    else
                    {
                        prp.Status = "Pending";
                    }
                    await _unitOfWork.PickRequestParts.UpdateAsync(prp);
                }
            }
            else
            {
                // Removed from old part
                if (oldPartId.HasValue)
                {
                    var oldPrp = await _unitOfWork.PickRequestParts.GetByIdAsync(oldPartId.Value);
                    if (oldPrp != null)
                    {
                        oldPrp.PickedQuantity = Math.Max(0, oldPrp.PickedQuantity - oldQuantity);
                        oldPrp.Status = oldPrp.PickedQuantity >= oldPrp.RequiredQuantity ? "Completed" :
                                        oldPrp.PickedQuantity > 0 ? "PartiallyPicked" : "Pending";
                        await _unitOfWork.PickRequestParts.UpdateAsync(oldPrp);
                    }
                }
                // Added to new part
                if (item.PickRequestPartId.HasValue)
                {
                    var newPrp = await _unitOfWork.PickRequestParts.GetByIdAsync(item.PickRequestPartId.Value);
                    if (newPrp != null)
                    {
                        newPrp.PickedQuantity += item.Quantity;
                        newPrp.Status = newPrp.PickedQuantity >= newPrp.RequiredQuantity ? "Completed" :
                                        newPrp.PickedQuantity > 0 ? "PartiallyPicked" : "Pending";
                        await _unitOfWork.PickRequestParts.UpdateAsync(newPrp);
                    }
                }
            }

            await _unitOfWork.Picks.UpdateAsync(existingPick);
            updatedPicks.Add(existingPick);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated {Count} pick records", updatedPicks.Count);

        var resultDtos = new List<PickDto>();
        foreach (var p in updatedPicks)
        {
            var fullPick = await _unitOfWork.Picks.GetByIdAsync(p.Id);
            if (fullPick != null)
            {
                resultDtos.Add(new PickDto
                {
                    Id = fullPick.Id,
                    PickRequestId = fullPick.PickRequestId,
                    RequestNumber = fullPick.PickRequest?.RequestNumber,
                    PickRequestPartId = fullPick.PickRequestPartId,
                    PartId = fullPick.PartId,
                    PartCode = fullPick.Part?.Code,
                    PartName = fullPick.Part?.Name,
                    Barcode = fullPick.Barcode,
                    Quantity = fullPick.Quantity,
                    PickedBy = fullPick.PickedBy,
                    PickerName = fullPick.Picker?.FullName,
                    DriverId = fullPick.DriverId,
                    DriverName = fullPick.Driver?.FullName,
                    VehicleId = fullPick.VehicleId,
                    VehicleNumber = fullPick.Vehicle?.VehicleNumber,
                    PlateNumber = fullPick.Vehicle?.PlateNumber,
                    PickDate = fullPick.PickDate,
                    Status = fullPick.Status,
                    Notes = fullPick.Notes,
                    CreatedAt = fullPick.CreatedAt,
                    UpdatedAt = fullPick.UpdatedAt
                });
            }
        }

        return resultDtos;
    }
}
