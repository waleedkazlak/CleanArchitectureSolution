using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssembliesWithFilterQueryHandler : IRequestHandler<GetFieldAssembliesWithFilterQuery, PaginatedResultDto<FieldAssemblyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetFieldAssembliesWithFilterQueryHandler> _logger;

    public GetFieldAssembliesWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetFieldAssembliesWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<FieldAssemblyDto>> Handle(GetFieldAssembliesWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetFieldAssembliesWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var assemblies = await _unitOfWork.FieldAssemblies.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            assemblies = assemblies.Where(fa =>
                (fa.FieldJob != null && fa.FieldJob.JobNumber.ToLower().Contains(searchTermLower)) ||
                (fa.ProductVariant != null && (fa.ProductVariant.Code.ToLower().Contains(searchTermLower) || (fa.ProductVariant.Barcode != null && fa.ProductVariant.Barcode.ToLower().Contains(searchTermLower)))) ||
                (fa.ProductBarcode != null && fa.ProductBarcode.ToLower().Contains(searchTermLower)) ||
                (fa.Technician != null && fa.Technician.FullName.ToLower().Contains(searchTermLower)) ||
                (fa.Supervisor != null && fa.Supervisor.FullName.ToLower().Contains(searchTermLower)) ||
                (fa.Notes != null && fa.Notes.ToLower().Contains(searchTermLower)) ||
                fa.Status.ToLower().Contains(searchTermLower)
            ).ToList();
        }

        if (filter.FieldJobId.HasValue)
        {
            assemblies = assemblies.Where(fa => fa.FieldJobId == filter.FieldJobId.Value).ToList();
        }

        if (filter.ProductVariantId.HasValue)
        {
            assemblies = assemblies.Where(fa => fa.ProductVariantId == filter.ProductVariantId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.ProductBarcode))
        {
            var barcodeLower = filter.ProductBarcode.ToLower();
            assemblies = assemblies.Where(fa => fa.ProductBarcode != null && fa.ProductBarcode.ToLower().Contains(barcodeLower)).ToList();
        }

        if (filter.TechnicianId.HasValue)
        {
            assemblies = assemblies.Where(fa => fa.TechnicianId == filter.TechnicianId.Value).ToList();
        }

        if (filter.SupervisorId.HasValue)
        {
            assemblies = assemblies.Where(fa => fa.SupervisorId == filter.SupervisorId.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            var statusLower = filter.Status.ToLower();
            assemblies = assemblies.Where(fa => fa.Status.ToLower().Contains(statusLower)).ToList();
        }

        if (filter.Verified.HasValue)
        {
            assemblies = assemblies.Where(fa => fa.Verified == filter.Verified.Value).ToList();
        }

        if (filter.FromDate.HasValue)
        {
            assemblies = assemblies.Where(fa => (fa.AssemblyDate ?? fa.CreatedAt) >= filter.FromDate.Value).ToList();
        }

        if (filter.ToDate.HasValue)
        {
            assemblies = assemblies.Where(fa => (fa.AssemblyDate ?? fa.CreatedAt) <= filter.ToDate.Value).ToList();
        }

        assemblies = ApplySort(assemblies.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = assemblies.Count();

        var paginatedItems = assemblies
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedItems.Select(fa => new FieldAssemblyDto
        {
            Id = fa.Id,
            FieldJobId = fa.FieldJobId,
            JobNumber = fa.FieldJob?.JobNumber,
            ProductVariantId = fa.ProductVariantId,
            ProductVariantCode = fa.ProductVariant?.Code,
            ProductBarcode = fa.ProductBarcode,
            Quantity = fa.Quantity,
            AssemblyDate = fa.AssemblyDate,
            Status = fa.Status,
            TechnicianId = fa.TechnicianId,
            TechnicianName = fa.Technician?.FullName,
            SupervisorId = fa.SupervisorId,
            SupervisorName = fa.Supervisor?.FullName,
            Verified = fa.Verified,
            VerifiedAt = fa.VerifiedAt,
            Notes = fa.Notes,
            CreatedAt = fa.CreatedAt,
            UpdatedAt = fa.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<FieldAssemblyDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.FieldAssembly> ApplySort(List<Domain.Entities.FieldAssembly> items, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "fieldjobid" => isDescending
                ? items.OrderByDescending(x => x.FieldJobId).ToList()
                : items.OrderBy(x => x.FieldJobId).ToList(),

            "productvariantid" => isDescending
                ? items.OrderByDescending(x => x.ProductVariantId).ToList()
                : items.OrderBy(x => x.ProductVariantId).ToList(),

            "quantity" => isDescending
                ? items.OrderByDescending(x => x.Quantity).ToList()
                : items.OrderBy(x => x.Quantity).ToList(),

            "assemblydate" => isDescending
                ? items.OrderByDescending(x => x.AssemblyDate).ToList()
                : items.OrderBy(x => x.AssemblyDate).ToList(),

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
