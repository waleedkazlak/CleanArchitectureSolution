using CleanSample.Application.DTOs;
using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.FieldAssembly;

public class CreateFieldAssemblyCommandHandler : IRequestHandler<CreateFieldAssemblyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateFieldAssemblyCommandHandler> _logger;
    private readonly Services.IWorkflowOrchestratorService _workflowOrchestrator;

    public CreateFieldAssemblyCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateFieldAssemblyCommandHandler> logger,
        Services.IWorkflowOrchestratorService workflowOrchestrator)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _workflowOrchestrator = workflowOrchestrator;
    }

    public async Task<bool> Handle(CreateFieldAssemblyCommand request, CancellationToken cancellationToken)
    {
        var itemsToProcess = new List<CreateFieldAssemblyItemDto>();

        if (request.Items != null && request.Items.Any())
        {
            itemsToProcess.AddRange(request.Items);
        }
        else if (request.FieldJobId > 0 && request.ProductId > 0)
        {
            itemsToProcess.Add(new CreateFieldAssemblyItemDto
            {
                Id = request.Id,
                FieldJobId = request.FieldJobId,
                ProductId = request.ProductId,
                ProductBarcode = request.ProductBarcode,
                Quantity = request.Quantity,
                AssemblyDate = request.AssemblyDate,
                Status = request.Status,
                TechnicianId = request.TechnicianId,
                SupervisorId = request.SupervisorId,
                Verified = request.Verified,
                VerifiedAt = request.VerifiedAt,
                Notes = request.Notes
            });
        }

        if (!itemsToProcess.Any())
        {
            _logger.LogWarning("CreateFieldAssemblyCommand called with no items to process");
            return false;
        }

        _logger.LogInformation("Processing {Count} FieldAssembly items (create/update)", itemsToProcess.Count);

        var affectedFieldJobIds = new HashSet<long>();

        foreach (var item in itemsToProcess)
        {
            Domain.Entities.FieldAssembly? existing = null;
            affectedFieldJobIds.Add(item.FieldJobId);

            if (item.Id.HasValue && item.Id.Value > 0)
            {
                existing = await _unitOfWork.FieldAssemblies.GetByIdAsync(item.Id.Value);
            }

            if (existing != null)
            {
                existing.FieldJobId = item.FieldJobId;
                existing.ProductId = item.ProductId;
                existing.ProductBarcode = item.ProductBarcode;
                existing.Quantity = item.Quantity;
                existing.AssemblyDate = item.AssemblyDate;
                if (item.Status.HasValue)
                {
                    existing.Status = item.Status.Value;
                }
                existing.TechnicianId = item.TechnicianId;
                existing.SupervisorId = item.SupervisorId;
                existing.Verified = item.Verified;
                existing.VerifiedAt = item.Verified ? (item.VerifiedAt ?? DateTime.UtcNow) : null;
                existing.Notes = item.Notes;
                existing.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.FieldAssemblies.UpdateAsync(existing);
                _logger.LogInformation("Updated existing FieldAssembly with Id: {Id}", existing.Id);
            }
            else
            {
                var newAssembly = new Domain.Entities.FieldAssembly
                {
                    FieldJobId = item.FieldJobId,
                    ProductId = item.ProductId,
                    ProductBarcode = item.ProductBarcode,
                    Quantity = item.Quantity,
                    AssemblyDate = item.AssemblyDate,
                    Status = item.Status ?? (int)Domain.Enums.FieldAssemblyStatusEnum.InProgress,
                    TechnicianId = item.TechnicianId,
                    SupervisorId = item.SupervisorId,
                    Verified = item.Verified,
                    VerifiedAt = item.Verified ? (item.VerifiedAt ?? DateTime.UtcNow) : null,
                    Notes = item.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                var newId = await _unitOfWork.FieldAssemblies.AddAsync(newAssembly);
                _logger.LogInformation("Created new FieldAssembly with Id: {Id}", newId);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Check if all FieldAssemblies for the related LoadRequest are Completed (2)
        foreach (var jobId in affectedFieldJobIds)
        {
            var job = await _unitOfWork.FieldJobs.GetByIdAsync(jobId);
            if (job != null)
            {
                await _workflowOrchestrator.RecalculateLoadRequestStatusAfterFieldAssembliesChangeAsync(job.LoadRequestId, cancellationToken);
            }
        }

        _logger.LogInformation("Successfully processed {Count} FieldAssembly items", itemsToProcess.Count);
        return true;
    }
}
