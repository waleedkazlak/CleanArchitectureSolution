using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.FieldAssembly;

public class DeleteFieldAssemblyCommandHandler : IRequestHandler<DeleteFieldAssemblyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteFieldAssemblyCommandHandler> _logger;

    public DeleteFieldAssemblyCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteFieldAssemblyCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteFieldAssemblyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteFieldAssemblyCommand for ID: {FieldAssemblyId}", request.Id);

        var existingAssembly = await _unitOfWork.FieldAssemblies.GetByIdAsync(request.Id);
        if (existingAssembly == null)
        {
            _logger.LogWarning("FieldAssembly with ID {FieldAssemblyId} not found", request.Id);
            return false;
        }

        await _unitOfWork.FieldAssemblies.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully deleted FieldAssembly with ID: {FieldAssemblyId}", request.Id);
        return true;
    }
}
