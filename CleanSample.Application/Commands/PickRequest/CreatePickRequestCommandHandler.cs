using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.PickRequest;

public class CreatePickRequestCommandHandler : IRequestHandler<CreatePickRequestCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreatePickRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(CreatePickRequestCommand request, CancellationToken cancellationToken)
    {
        var pickRequest = new CleanSample.Domain.Entities.PickRequest
        {
            RequestNumber = request.RequestNumber,
            OrderId = request.OrderId,
            ClientId = request.ClientId,
            ClientLocationId = request.ClientLocationId,
            RequestedBy = request.RequestedBy,
            RequestDate = request.RequestDate ?? DateTime.UtcNow,
            ExecutionDate = request.ExecutionDate,
            Status = string.IsNullOrWhiteSpace(request.Status) ? "Created" : request.Status,
            DestinationAddress = request.DestinationAddress,
            DestinationCity = request.DestinationCity,
            Description = request.Description,
            DriverId = request.DriverId,
            VehicleId = request.VehicleId,
            Verified = request.Verified
        };

        foreach (var line in request.PickRequestLines)
        {
            var lineEntity = new PickRequestLine
            {
                ProductVariantId = line.ProductVariantId,
                Quantity = line.Quantity,
                CreatedAt = DateTime.UtcNow
            };

            // Fetch ProductBOM for this ProductVariant to automatically generate PickRequestParts
            var boms = await _unitOfWork.ProductBOMs.GetByProductVariantIdAsync(line.ProductVariantId);
            foreach (var bom in boms)
            {
                lineEntity.PickRequestParts.Add(new PickRequestPart
                {
                    PartId = bom.PartId,
                    RequiredQuantity = line.Quantity * bom.Quantity,
                    PickedQuantity = 0,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                });
            }

            pickRequest.PickRequestLines.Add(lineEntity);
        }

        var pickRequestId = await _unitOfWork.PickRequests.AddAsync(pickRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return pickRequestId;
    }
}
