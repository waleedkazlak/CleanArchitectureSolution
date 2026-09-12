using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleOffload;

public class CreateVehicleOffloadCommandHandler : IRequestHandler<CreateVehicleOffloadCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVehicleOffloadCommandHandler> _logger;
    private readonly Services.IWorkflowOrchestratorService _workflowOrchestrator;

    public CreateVehicleOffloadCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateVehicleOffloadCommandHandler> logger,
        Services.IWorkflowOrchestratorService workflowOrchestrator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _workflowOrchestrator = workflowOrchestrator;
    }

    public async Task<long> Handle(CreateVehicleOffloadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateVehicleOffloadCommand for LoadRequestId: {LoadRequestId}, VehicleId: {VehicleId}",
            request.LoadRequestId, request.VehicleId);

        var vehicleOffload = new Domain.Entities.VehicleOffload
        {
            LoadRequestId = request.LoadRequestId,
            PartId = request.PartId,
            VehicleId = request.VehicleId,
            DriverId = request.DriverId,
            Barcode = request.Barcode,
            OffloadDate = request.OffloadDate ?? DateTime.UtcNow,
            Status = request.Status == 0 ? (int)Domain.Enums.VehicleOffloadStatusEnum.Good : request.Status,
            Verified = request.Verified,
            VerifiedBy = request.VerifiedBy,
            VerifiedAt = request.Verified ? (request.VerifiedAt ?? DateTime.UtcNow) : null,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.VehicleOffloads.AddAsync(vehicleOffload);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Trigger workflow recalculation for LoadRequest
        await _workflowOrchestrator.RecalculateLoadRequestStatusAfterVehicleOffloadsChangeAsync(request.LoadRequestId, cancellationToken);

        _logger.LogInformation("Successfully created VehicleOffload with ID: {VehicleOffloadId}", vehicleOffload.Id);

        return vehicleOffload.Id;
    }
}
