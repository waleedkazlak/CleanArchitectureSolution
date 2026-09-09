using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.FieldAssembly;

public class CreateFieldAssemblyCommandHandler : IRequestHandler<CreateFieldAssemblyCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateFieldAssemblyCommandHandler> _logger;

    public CreateFieldAssemblyCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateFieldAssemblyCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<long> Handle(CreateFieldAssemblyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateFieldAssemblyCommand for FieldJobId: {FieldJobId}, ProductVariantId: {ProductVariantId}",
            request.FieldJobId, request.ProductVariantId);

        var fieldAssembly = new Domain.Entities.FieldAssembly
        {
            FieldJobId = request.FieldJobId,
            ProductVariantId = request.ProductVariantId,
            ProductBarcode = request.ProductBarcode,
            Quantity = request.Quantity,
            AssemblyDate = request.AssemblyDate,
            Status = string.IsNullOrWhiteSpace(request.Status) ? "Pending" : request.Status,
            TechnicianId = request.TechnicianId,
            SupervisorId = request.SupervisorId,
            Verified = request.Verified,
            VerifiedAt = request.Verified ? (request.VerifiedAt ?? DateTime.UtcNow) : null,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.FieldAssemblies.AddAsync(fieldAssembly);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully created FieldAssembly with ID: {FieldAssemblyId}", fieldAssembly.Id);

        return fieldAssembly.Id;
    }
}
