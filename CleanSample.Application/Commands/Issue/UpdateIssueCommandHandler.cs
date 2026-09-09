using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Issue;

public class UpdateIssueCommandHandler : IRequestHandler<UpdateIssueCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateIssueCommandHandler> _logger;

    public UpdateIssueCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateIssueCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateIssueCommand for ID: {IssueId}", request.Id);

        var existingIssue = await _unitOfWork.Issues.GetByIdAsync(request.Id);
        if (existingIssue == null)
        {
            _logger.LogWarning("Issue with ID {IssueId} not found", request.Id);
            return false;
        }

        existingIssue.PickRequestId = request.PickRequestId;
        existingIssue.FieldJobId = request.FieldJobId;
        existingIssue.FieldAssemblyId = request.FieldAssemblyId;
        existingIssue.IssueType = request.IssueType;
        existingIssue.Description = request.Description;
        if (!string.IsNullOrWhiteSpace(request.Severity))
        {
            existingIssue.Severity = request.Severity;
        }
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            existingIssue.Status = request.Status;
        }
        existingIssue.ReportedBy = request.ReportedBy;
        if (request.ReportedAt.HasValue)
        {
            existingIssue.ReportedAt = request.ReportedAt.Value;
        }
        existingIssue.ResolvedBy = request.ResolvedBy;
        existingIssue.ResolvedAt = request.ResolvedAt;
        existingIssue.ResolutionNotes = request.ResolutionNotes;
        existingIssue.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Issues.UpdateAsync(existingIssue);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully updated Issue with ID: {IssueId}", existingIssue.Id);
        return true;
    }
}
