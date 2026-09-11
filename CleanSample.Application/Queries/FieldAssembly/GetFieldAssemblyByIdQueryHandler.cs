using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.FieldAssembly;

public class GetFieldAssemblyByIdQueryHandler : IRequestHandler<GetFieldAssemblyByIdQuery, FieldAssemblyDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFieldAssemblyByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<FieldAssemblyDto?> Handle(GetFieldAssemblyByIdQuery request, CancellationToken cancellationToken)
    {
        var fa = await _unitOfWork.FieldAssemblies.GetByIdAsync(request.Id);
        if (fa == null)
        {
            return null;
        }

        return new FieldAssemblyDto
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
        };
    }
}
