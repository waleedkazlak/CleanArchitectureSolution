using CleanSample.Application.DTOs;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Pick;

public class CreatePicksCommandHandler : IRequestHandler<CreatePicksCommand, List<PickDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatePicksCommandHandler> _logger;

    public CreatePicksCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePicksCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<PickDto>> Handle(CreatePicksCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreatePicksCommand with {Count} items", request.Picks?.Count ?? 0);

        if (request.Picks == null || !request.Picks.Any())
        {
            return new List<PickDto>();
        }

        var newPicks = new List<Domain.Entities.Pick>();

        foreach (var item in request.Picks)
        {
            long? pickRequestPartId = item.PickRequestPartId;

            // If PickRequestPartId is not specified, attempt to resolve from PickRequestParts matching PickRequestId & PartId
            if (!pickRequestPartId.HasValue)
            {
                var relatedParts = await _unitOfWork.PickRequestParts.GetByPickRequestIdAsync(item.PickRequestId);
                var matchingPart = relatedParts.FirstOrDefault(p => p.PartId == item.PartId);
                if (matchingPart != null)
                {
                    pickRequestPartId = matchingPart.Id;
                }
            }

            var pick = new Domain.Entities.Pick
            {
                PickRequestId = item.PickRequestId,
                PickRequestPartId = pickRequestPartId,
                PartId = item.PartId,
                Barcode = item.Barcode,
                Quantity = item.Quantity,
                PickedBy = item.PickedBy,
                DriverId = item.DriverId,
                VehicleId = item.VehicleId,
                PickDate = item.PickDate ?? DateTime.UtcNow,
                Status = string.IsNullOrWhiteSpace(item.Status) ? "Picked" : item.Status,
                Notes = item.Notes,
                CreatedAt = DateTime.UtcNow
            };

            newPicks.Add(pick);

            // Update PickRequestPart PickedQuantity and Status if linked
            if (pickRequestPartId.HasValue)
            {
                var prp = await _unitOfWork.PickRequestParts.GetByIdAsync(pickRequestPartId.Value);
                if (prp != null)
                {
                    prp.PickedQuantity += item.Quantity;
                    if (prp.PickedQuantity >= prp.RequiredQuantity)
                    {
                        prp.Status = "Completed";
                    }
                    else if (prp.PickedQuantity > 0)
                    {
                        prp.Status = "PartiallyPicked";
                    }
                    await _unitOfWork.PickRequestParts.UpdateAsync(prp);
                }
            }
        }

        await _unitOfWork.Picks.AddRangeAsync(newPicks);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created {Count} pick records", newPicks.Count);

        // Map to DTOs
        var resultDtos = new List<PickDto>();
        foreach (var p in newPicks)
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
            else
            {
                resultDtos.Add(new PickDto
                {
                    Id = p.Id,
                    PickRequestId = p.PickRequestId,
                    PickRequestPartId = p.PickRequestPartId,
                    PartId = p.PartId,
                    Barcode = p.Barcode,
                    Quantity = p.Quantity,
                    PickedBy = p.PickedBy,
                    DriverId = p.DriverId,
                    VehicleId = p.VehicleId,
                    PickDate = p.PickDate,
                    Status = p.Status,
                    Notes = p.Notes,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
        }

        return resultDtos;
    }
}
