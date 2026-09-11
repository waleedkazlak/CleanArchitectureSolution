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
            OrderId = request.OrderId,
            ClientId = request.ClientId,
            ClientLocationId = request.ClientLocationId,
            RequestedBy = request.RequestedBy,
            RequestDate = request.RequestDate ?? DateTime.UtcNow,
            ExecutionDate = request.ExecutionDate,
            Status = request.Status,
            DestinationAddress = request.DestinationAddress,
            DestinationCity = request.DestinationCity,
            Description = request.Description,
            DriverId = request.DriverId,
            VehicleId = request.VehicleId,
            Verified = request.Verified
        };

        foreach (var line in request.LoadRequestLines)
        {
            var lineEntity = new CleanSample.Domain.Entities.LoadRequestLine
            {
                LoadRequest = loadRequest,
                ProductId = line.ProductId,
                Quantity = line.Quantity,
                CreatedAt = DateTime.UtcNow
            };

            // Fetch ProductBOM for this Product to automatically generate LoadRequestParts
            var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(line.ProductId);
            foreach (var bom in boms)
            {
                var partEntity = new LoadRequestPart
                {
                    LoadRequest = loadRequest,
                    LoadRequestLine = lineEntity,
                    PartId = bom.PartId,
                    RequiredQuantity = line.Quantity * bom.Quantity,
                    LoadedQuantity = 0,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow
                };
                lineEntity.LoadRequestParts.Add(partEntity);
                loadRequest.LoadRequestParts.Add(partEntity);
            }

            loadRequest.LoadRequestLines.Add(lineEntity);
        }

        var loadRequestId = await _unitOfWork.LoadRequests.AddAsync(loadRequest);

        return loadRequestId;
    }
}
