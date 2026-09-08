using MediatR;
using CleanSample.Domain.Interfaces;

namespace CleanSample.Application.Commands.Vehicle;

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteVehicleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _unitOfWork.Vehicles.GetByIdAsync(request.Id);
        if (vehicle == null)
        {
            return false;
        }

        await _unitOfWork.Vehicles.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
