using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.FieldJob;

public class CreateFieldJobCommandHandler : IRequestHandler<CreateFieldJobCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateFieldJobCommandHandler> _logger;

    public CreateFieldJobCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateFieldJobCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<long> Handle(CreateFieldJobCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateFieldJobCommand for LoadRequestId: {LoadRequestId}",
            request.LoadRequestId);

        var fieldJob = new Domain.Entities.FieldJob
        {
            LoadRequestId = request.LoadRequestId,
            ClientId = request.ClientId,
            ClientLocationId = request.ClientLocationId,
            TechnicianId = request.TechnicianId,
            SupervisorId = request.SupervisorId,
            ScheduledDate = request.ScheduledDate,
            StartDate = request.StartDate,
            CompletionDate = request.CompletionDate,
            Status = string.IsNullOrWhiteSpace(request.Status) ? "Scheduled" : request.Status,
            Verified = request.Verified,
            VerifiedAt = request.Verified ? (request.VerifiedAt ?? DateTime.UtcNow) : null,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.FieldJobs.AddAsync(fieldJob);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created FieldJob with ID: {FieldJobId}", fieldJob.Id);

        return fieldJob.Id;
    }
}
