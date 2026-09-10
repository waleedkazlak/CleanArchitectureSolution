using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.FieldJob;

public class GetFieldJobsBySupervisorIdQueryHandler : IRequestHandler<GetFieldJobsBySupervisorIdQuery, List<FieldJobDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFieldJobsBySupervisorIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<FieldJobDto>> Handle(GetFieldJobsBySupervisorIdQuery request, CancellationToken cancellationToken)
    {
        var jobs = await _unitOfWork.FieldJobs.GetBySupervisorIdAsync(request.SupervisorId);

        return jobs.Select(fj => new FieldJobDto
        {
            Id = fj.Id,
            JobNumber = fj.JobNumber,
            LoadRequestId = fj.LoadRequestId,
            LoadRequestNumber = fj.LoadRequest?.RequestNumber,
            ClientId = fj.ClientId,
            ClientName = fj.Client?.Name,
            ClientLocationId = fj.ClientLocationId,
            LocationName = fj.ClientLocation?.Name,
            TechnicianId = fj.TechnicianId,
            TechnicianName = fj.Technician?.FullName,
            SupervisorId = fj.SupervisorId,
            SupervisorName = fj.Supervisor?.FullName,
            ScheduledDate = fj.ScheduledDate,
            StartDate = fj.StartDate,
            CompletionDate = fj.CompletionDate,
            Status = fj.Status,
            Verified = fj.Verified,
            VerifiedAt = fj.VerifiedAt,
            Notes = fj.Notes,
            CreatedAt = fj.CreatedAt,
            UpdatedAt = fj.UpdatedAt
        }).ToList();
    }
}
