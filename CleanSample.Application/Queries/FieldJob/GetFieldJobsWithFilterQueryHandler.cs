using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.FieldJob;

public class GetFieldJobsWithFilterQueryHandler : IRequestHandler<GetFieldJobsWithFilterQuery, PaginatedResultDto<FieldJobDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetFieldJobsWithFilterQueryHandler> _logger;

    public GetFieldJobsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetFieldJobsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<FieldJobDto>> Handle(GetFieldJobsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetFieldJobsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var jobs = await _unitOfWork.FieldJobs.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            jobs = jobs.Where(fj =>
                fj.JobNumber.ToLower().Contains(searchTermLower) ||
                (fj.PickRequest != null && fj.PickRequest.RequestNumber.ToLower().Contains(searchTermLower)) ||
                (fj.Client != null && fj.Client.Name.ToLower().Contains(searchTermLower)) ||
                (fj.ClientLocation != null && fj.ClientLocation.Name.ToLower().Contains(searchTermLower)) ||
                (fj.Technician != null && fj.Technician.FullName.ToLower().Contains(searchTermLower)) ||
                (fj.Supervisor != null && fj.Supervisor.FullName.ToLower().Contains(searchTermLower)) ||
                (fj.Notes != null && fj.Notes.ToLower().Contains(searchTermLower)) ||
                fj.Status.ToLower().Contains(searchTermLower)
            ).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.JobNumber))
        {
            var jobNumLower = filter.JobNumber.ToLower();
            jobs = jobs.Where(fj => fj.JobNumber.ToLower().Contains(jobNumLower)).ToList();
        }

        if (filter.PickRequestId.HasValue)
        {
            jobs = jobs.Where(fj => fj.PickRequestId == filter.PickRequestId.Value).ToList();
        }

        if (filter.ClientId.HasValue)
        {
            jobs = jobs.Where(fj => fj.ClientId == filter.ClientId.Value).ToList();
        }

        if (filter.ClientLocationId.HasValue)
        {
            jobs = jobs.Where(fj => fj.ClientLocationId == filter.ClientLocationId.Value).ToList();
        }

        if (filter.TechnicianId.HasValue)
        {
            jobs = jobs.Where(fj => fj.TechnicianId == filter.TechnicianId.Value).ToList();
        }

        if (filter.SupervisorId.HasValue)
        {
            jobs = jobs.Where(fj => fj.SupervisorId == filter.SupervisorId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var statusLower = filter.Status.ToLower();
            jobs = jobs.Where(fj => fj.Status.ToLower().Contains(statusLower)).ToList();
        }

        if (filter.Verified.HasValue)
        {
            jobs = jobs.Where(fj => fj.Verified == filter.Verified.Value).ToList();
        }

        if (filter.FromDate.HasValue)
        {
            jobs = jobs.Where(fj => (fj.ScheduledDate ?? fj.CreatedAt) >= filter.FromDate.Value).ToList();
        }

        if (filter.ToDate.HasValue)
        {
            jobs = jobs.Where(fj => (fj.ScheduledDate ?? fj.CreatedAt) <= filter.ToDate.Value).ToList();
        }

        jobs = ApplySort(jobs.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = jobs.Count();

        var paginatedItems = jobs
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(fj => new FieldJobDto
        {
            Id = fj.Id,
            JobNumber = fj.JobNumber,
            PickRequestId = fj.PickRequestId,
            PickRequestNumber = fj.PickRequest?.RequestNumber,
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

        return new PaginatedResultDto<FieldJobDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.FieldJob> ApplySort(List<Domain.Entities.FieldJob> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "jobnumber" => isDescending
                ? items.OrderByDescending(x => x.JobNumber).ToList()
                : items.OrderBy(x => x.JobNumber).ToList(),

            "scheduleddate" => isDescending
                ? items.OrderByDescending(x => x.ScheduledDate).ToList()
                : items.OrderBy(x => x.ScheduledDate).ToList(),

            "startdate" => isDescending
                ? items.OrderByDescending(x => x.StartDate).ToList()
                : items.OrderBy(x => x.StartDate).ToList(),

            "completiondate" => isDescending
                ? items.OrderByDescending(x => x.CompletionDate).ToList()
                : items.OrderBy(x => x.CompletionDate).ToList(),

            "pickrequestid" => isDescending
                ? items.OrderByDescending(x => x.PickRequestId).ToList()
                : items.OrderBy(x => x.PickRequestId).ToList(),

            "clientid" => isDescending
                ? items.OrderByDescending(x => x.ClientId).ToList()
                : items.OrderBy(x => x.ClientId).ToList(),

            "status" => isDescending
                ? items.OrderByDescending(x => x.Status).ToList()
                : items.OrderBy(x => x.Status).ToList(),

            "createdat" => isDescending
                ? items.OrderByDescending(x => x.CreatedAt).ToList()
                : items.OrderBy(x => x.CreatedAt).ToList(),

            _ => items.OrderByDescending(x => x.CreatedAt).ToList()
        };
    }
}
