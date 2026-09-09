using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.VehicleLoad;

public class DeleteVehicleLoadCommandHandler : IRequestHandler<DeleteVehicleLoadCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteVehicleLoadCommandHandler> _logger;

    public DeleteVehicleLoadCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteVehicleLoadCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteVehicleLoadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteVehicleLoadCommand for ID: {VehicleLoadId}", request.Id);

        var existingLoad = await _unitOfWork.VehicleLoads.GetByIdAsync(request.Id);
        if (existingLoad == null)
        {
            _logger.LogWarning("VehicleLoad with ID {VehicleLoadId} not found", request.Id);
            return false;
        }

        await _unitOfWork.VehicleLoads.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully deleted VehicleLoad with ID: {VehicleLoadId}", request.Id);
        return true;
    }
}
