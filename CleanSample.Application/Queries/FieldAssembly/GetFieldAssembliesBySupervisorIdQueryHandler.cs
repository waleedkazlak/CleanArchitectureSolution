using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssembliesBySupervisorIdQueryHandler : IRequestHandler<GetFieldAssembliesBySupervisorIdQuery, List<FieldAssemblyDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFieldAssembliesBySupervisorIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<FieldAssemblyDto>> Handle(GetFieldAssembliesBySupervisorIdQuery request, CancellationToken cancellationToken)
    {
        var assemblies = await _unitOfWork.FieldAssemblies.GetBySupervisorIdAsync(request.SupervisorId);

        return assemblies.Select(fa => new FieldAssemblyDto
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
    }
}
