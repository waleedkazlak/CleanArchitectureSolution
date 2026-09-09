using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Issue;

public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteIssueCommandHandler> _logger;

    public DeleteIssueCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteIssueCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteIssueCommand for ID: {IssueId}", request.Id);

        var existingIssue = await _unitOfWork.Issues.GetByIdAsync(request.Id);
        if (existingIssue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found", request.Id);
            return false;
        }

        await _unitOfWork.Issues.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully deleted Issue with ID: {IssueId}", request.Id);
        return true;
    }
}
