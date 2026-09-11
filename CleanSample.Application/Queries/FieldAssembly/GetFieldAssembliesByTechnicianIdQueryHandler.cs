using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssembliesByTechnicianIdQueryHandler : IRequestHandler<GetFieldAssembliesByTechnicianIdQuery, List<FieldAssemblyDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFieldAssembliesByTechnicianIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<FieldAssemblyDto>> Handle(GetFieldAssembliesByTechnicianIdQuery request, CancellationToken cancellationToken)
    {
        var assemblies = await _unitOfWork.FieldAssemblies.GetByTechnicianIdAsync(request.TechnicianId);

        return assemblies.Select(fa => new FieldAssemblyDto
        {
            Id = fa.Id,
            FieldJobId = fa.FieldJobId,
            ProductId = fa.ProductId,
            ProductName = fa.Product?.Name,
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
