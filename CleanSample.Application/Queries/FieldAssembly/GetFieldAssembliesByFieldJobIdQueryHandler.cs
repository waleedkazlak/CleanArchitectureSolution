using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssembliesByFieldJobIdQueryHandler : IRequestHandler<GetFieldAssembliesByFieldJobIdQuery, List<FieldAssemblyDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFieldAssembliesByFieldJobIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<FieldAssemblyDto>> Handle(GetFieldAssembliesByFieldJobIdQuery request, CancellationToken cancellationToken)
    {
        var assemblies = await _unitOfWork.FieldAssemblies.GetByFieldJobIdAsync(request.FieldJobId);

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
