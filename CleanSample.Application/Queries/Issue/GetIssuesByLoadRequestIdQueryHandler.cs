using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Issue;

public class GetIssuesByLoadRequestIdQueryHandler : IRequestHandler<GetIssuesByLoadRequestIdQuery, List<IssueDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetIssuesByLoadRequestIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<IssueDto>> Handle(GetIssuesByLoadRequestIdQuery request, CancellationToken cancellationToken)
    {
        var issues = await _unitOfWork.Issues.GetByLoadRequestIdAsync(request.LoadRequestId);

        return issues.Select(i => new IssueDto
        {
            Id = i.Id,
            LoadRequestId = i.LoadRequestId,
            FieldJobId = i.FieldJobId,
            FieldAssemblyId = i.FieldAssemblyId,
            IssueType = i.IssueType,
            Description = i.Description,
            Severity = i.Severity,
            Status = i.Status,
            ReportedBy = i.ReportedBy,
            ReportedByName = i.ReportedByUser?.FullName,
            ReportedAt = i.ReportedAt,
            ResolvedBy = i.ResolvedBy,
            ResolvedByName = i.ResolvedByUser?.FullName,
            ResolvedAt = i.ResolvedAt,
            ResolutionNotes = i.ResolutionNotes,
            CreatedAt = i.CreatedAt,
            UpdatedAt = i.UpdatedAt
        }).ToList();
    }
}
