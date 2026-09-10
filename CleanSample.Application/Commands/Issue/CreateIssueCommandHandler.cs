using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Issue;

public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateIssueCommandHandler> _logger;

    public CreateIssueCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateIssueCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<long> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateIssueCommand with IssueType: {IssueType}, Severity: {Severity}",
            request.IssueType, request.Severity);

        var issue = new Domain.Entities.Issue
        {
            LoadRequestId = request.LoadRequestId,
            FieldJobId = request.FieldJobId,
            FieldAssemblyId = request.FieldAssemblyId,
            IssueType = request.IssueType,
            Description = request.Description,
            Severity = string.IsNullOrWhiteSpace(request.Severity) ? "Medium" : request.Severity,
            Status = string.IsNullOrWhiteSpace(request.Status) ? "Open" : request.Status,
            ReportedBy = request.ReportedBy,
            ReportedAt = request.ReportedAt ?? DateTime.UtcNow,
            ResolvedBy = request.ResolvedBy,
            ResolvedAt = request.ResolvedAt,
            ResolutionNotes = request.ResolutionNotes,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Issues.AddAsync(issue);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created Issue with ID: {IssueId}", issue.Id);

        return issue.Id;
    }
}
