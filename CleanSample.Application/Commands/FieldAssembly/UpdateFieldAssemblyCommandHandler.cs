using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.FieldAssembly;

public class UpdateFieldAssemblyCommandHandler : IRequestHandler<UpdateFieldAssemblyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateFieldAssemblyCommandHandler> _logger;
    private readonly Services.IWorkflowOrchestratorService _workflowOrchestrator;

    public UpdateFieldAssemblyCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateFieldAssemblyCommandHandler> logger,
        Services.IWorkflowOrchestratorService workflowOrchestrator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _workflowOrchestrator = workflowOrchestrator;
    }

    public async Task<bool> Handle(UpdateFieldAssemblyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateFieldAssemblyCommand for ID: {FieldAssemblyId}", request.Id);

        var existingAssembly = await _unitOfWork.FieldAssemblies.GetByIdAsync(request.Id);
        if (existingAssembly == null)
        {
            _logger.LogWarning("FieldAssembly with ID {FieldAssemblyId} not found", request.Id);
            return false;
        }

        existingAssembly.FieldJobId = request.FieldJobId;
        existingAssembly.ProductId = request.ProductId;
        existingAssembly.ProductBarcode = request.ProductBarcode;
        existingAssembly.Quantity = request.Quantity;
        existingAssembly.AssemblyDate = request.AssemblyDate;
        existingAssembly.Status = request.Status;
        existingAssembly.TechnicianId = request.TechnicianId;
        existingAssembly.SupervisorId = request.SupervisorId;
        existingAssembly.Verified = request.Verified;
        existingAssembly.VerifiedAt = request.Verified ? (request.VerifiedAt ?? DateTime.UtcNow) : null;
        existingAssembly.Notes = request.Notes;
        existingAssembly.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.FieldAssemblies.UpdateAsync(existingAssembly);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Check if all FieldAssemblies for the related LoadRequest are Completed (2)
        var job = await _unitOfWork.FieldJobs.GetByIdAsync(existingAssembly.FieldJobId);
        if (job != null)
        {
            await _workflowOrchestrator.RecalculateLoadRequestStatusAfterFieldAssembliesChangeAsync(job.LoadRequestId, cancellationToken);
        }

        _logger.LogInformation("Successfully updated FieldAssembly with ID: {FieldAssemblyId}", existingAssembly.Id);
        return true;
    }
}
