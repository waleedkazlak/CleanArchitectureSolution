using CleanSample.Application.Services;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.LoadRequest;

public class CreateLoadRequestCommandHandler : IRequestHandler<CreateLoadRequestCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoadRequestBOMService _loadRequestBOMService;
    private readonly IWorkflowOrchestratorService _workflowOrchestrator;

    public CreateLoadRequestCommandHandler(
        IUnitOfWork unitOfWork,
        ILoadRequestBOMService loadRequestBOMService,
        IWorkflowOrchestratorService workflowOrchestrator)
    {
        _unitOfWork = unitOfWork;
        _loadRequestBOMService = loadRequestBOMService;
        _workflowOrchestrator = workflowOrchestrator;
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

            // If standalone (no OrderId), generate parts from BOM directly
            if (!request.OrderId.HasValue)
            {
                var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(line.ProductId);
                foreach (var bom in boms)
                {
                    var partEntity = new LoadRequestPart
                    {
                        LoadRequest = loadRequest,
                        LoadRequestLine = lineEntity,
                        ProductId = line.ProductId,
                        PartId = bom.PartId,
                        RequiredQuantity = line.Quantity * bom.Quantity,
                        LoadedQuantity = 0,
                        CreatedAt = DateTime.UtcNow
                    };
                    lineEntity.LoadRequestParts.Add(partEntity);
                    loadRequest.LoadRequestParts.Add(partEntity);
                }
            }

            loadRequest.LoadRequestLines.Add(lineEntity);
        }

        var loadRequestId = await _unitOfWork.LoadRequests.AddAsync(loadRequest);

        // If associated with an Order, check verification and quantities, and update Order.Status to Processing (4)
        if (loadRequest.OrderId.HasValue)
        {
            await _loadRequestBOMService.CheckAndGeneratePartsForOrderAsync(loadRequest.OrderId.Value, cancellationToken);
            await _workflowOrchestrator.RecalculateOrderStatusAfterLoadRequestChangeAsync(loadRequest.OrderId.Value, cancellationToken);
        }

        return loadRequestId;
    }
}
