using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.FieldJob;

public class UpdateFieldJobCommandHandler : IRequestHandler<UpdateFieldJobCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateFieldJobCommandHandler> _logger;

    public UpdateFieldJobCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateFieldJobCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateFieldJobCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateFieldJobCommand for ID: {FieldJobId}", request.Id);

        var existingJob = await _unitOfWork.FieldJobs.GetByIdAsync(request.Id);
        if (existingJob == null)
        {
            _logger.LogWarning("FieldJob with ID {FieldJobId} not found", request.Id);
            return false;
        }

        existingJob.LoadRequestId = request.LoadRequestId;
        existingJob.ClientId = request.ClientId;
        existingJob.ClientLocationId = request.ClientLocationId;
        existingJob.TechnicianId = request.TechnicianId;
        existingJob.SupervisorId = request.SupervisorId;
        existingJob.ScheduledDate = request.ScheduledDate;
        existingJob.StartDate = request.StartDate;
        existingJob.CompletionDate = request.CompletionDate;
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            existingJob.Status = request.Status;
        }
        existingJob.Verified = request.Verified;
        existingJob.VerifiedAt = request.Verified ? (request.VerifiedAt ?? DateTime.UtcNow) : null;
        existingJob.Notes = request.Notes;
        existingJob.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.FieldJobs.UpdateAsync(existingJob);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated FieldJob with ID: {FieldJobId}", existingJob.Id);
        return true;
    }
}
