using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.LoadRequest;

public class CreateLoadRequestCommandHandler : IRequestHandler<CreateLoadRequestCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateLoadRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<long> Handle(CreateLoadRequestCommand request, CancellationToken cancellationToken)
    {
        var loadRequest = new CleanSample.Domain.Entities.LoadRequest
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

        foreach (var line in request.LoadRequestLines)
        {
            var lineEntity = new LoadRequestLine
            {
                ProductVariantId = line.ProductVariantId,
                Quantity = line.Quantity,
                CreatedAt = DateTime.UtcNow
            };

            // Fetch ProductBOM for this ProductVariant to automatically generate LoadRequestParts
            var boms = await _unitOfWork.ProductBOMs.GetByProductVariantIdAsync(line.ProductVariantId);
            foreach (var bom in boms)
            {
                lineEntity.LoadRequestParts.Add(new LoadRequestPart
                {
                    PartId = bom.PartId,
                    RequiredQuantity = line.Quantity * bom.Quantity,
                    LoadedQuantity = 0,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                });
            }

            loadRequest.LoadRequestLines.Add(lineEntity);
        }

        var loadRequestId = await _unitOfWork.LoadRequests.AddAsync(loadRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return loadRequestId;
    }
}
