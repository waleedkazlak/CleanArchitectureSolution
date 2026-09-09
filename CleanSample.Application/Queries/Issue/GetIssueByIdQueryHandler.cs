using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Issue;

public class GetIssueByIdQueryHandler : IRequestHandler<GetIssueByIdQuery, IssueDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetIssueByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IssueDto?> Handle(GetIssueByIdQuery request, CancellationToken cancellationToken)
    {
        var issue = await _unitOfWork.Issues.GetByIdAsync(request.Id);
        if (issue == null)
        {
            return null;
        }

        return new IssueDto
        {
            Id = issue.Id,
            PickRequestId = issue.PickRequestId,
            PickRequestNumber = issue.PickRequest?.RequestNumber,
            FieldJobId = issue.FieldJobId,
            FieldJobNumber = issue.FieldJob?.JobNumber,
            FieldAssemblyId = issue.FieldAssemblyId,
            IssueType = issue.IssueType,
            Description = issue.Description,
            Severity = issue.Severity,
            Status = issue.Status,
            ReportedBy = issue.ReportedBy,
            ReportedByName = issue.ReportedByUser?.FullName,
            ReportedAt = issue.ReportedAt,
            ResolvedBy = issue.ResolvedBy,
            ResolvedByName = issue.ResolvedByUser?.FullName,
            ResolvedAt = issue.ResolvedAt,
            ResolutionNotes = issue.ResolutionNotes,
            CreatedAt = issue.CreatedAt,
            UpdatedAt = issue.UpdatedAt
        };
    }
}
