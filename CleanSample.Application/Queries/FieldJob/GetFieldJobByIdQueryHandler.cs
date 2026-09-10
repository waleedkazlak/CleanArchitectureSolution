using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.FieldJob;

public class GetFieldJobByIdQueryHandler : IRequestHandler<GetFieldJobByIdQuery, FieldJobDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFieldJobByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<FieldJobDto?> Handle(GetFieldJobByIdQuery request, CancellationToken cancellationToken)
    {
        var fj = await _unitOfWork.FieldJobs.GetByIdAsync(request.Id);
        if (fj == null)
        {
            return null;
        }

        return new FieldJobDto
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
        };
    }
}
