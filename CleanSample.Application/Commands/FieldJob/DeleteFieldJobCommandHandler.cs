using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.FieldJob;

public class DeleteFieldJobCommandHandler : IRequestHandler<DeleteFieldJobCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteFieldJobCommandHandler> _logger;

    public DeleteFieldJobCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteFieldJobCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteFieldJobCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteFieldJobCommand for ID: {FieldJobId}", request.Id);

        var existingJob = await _unitOfWork.FieldJobs.GetByIdAsync(request.Id);
        if (existingJob == null)
        {
            _logger.LogWarning("FieldJob with ID {FieldJobId} not found", request.Id);
            return false;
        }

        await _unitOfWork.FieldJobs.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully deleted FieldJob with ID: {FieldJobId}", request.Id);
        return true;
    }
}
