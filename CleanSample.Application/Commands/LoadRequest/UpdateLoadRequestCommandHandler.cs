using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.LoadRequest;

public class UpdateLoadRequestCommandHandler : IRequestHandler<UpdateLoadRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLoadRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateLoadRequestCommand request, CancellationToken cancellationToken)
    {
        var loadRequest = await _unitOfWork.LoadRequests.GetByIdAsync(request.Id);
        if (loadRequest == null)
        {
            return false;
        }

        loadRequest.RequestNumber = request.RequestNumber;
        loadRequest.OrderId = request.OrderId;
        loadRequest.ClientId = request.ClientId;
        loadRequest.ClientLocationId = request.ClientLocationId;
        loadRequest.RequestedBy = request.RequestedBy;
        loadRequest.RequestDate = request.RequestDate;
        loadRequest.ExecutionDate = request.ExecutionDate;
        loadRequest.Status = request.Status;
        loadRequest.DestinationAddress = request.DestinationAddress;
        loadRequest.DestinationCity = request.DestinationCity;
        loadRequest.Description = request.Description;
        loadRequest.DriverId = request.DriverId;
        loadRequest.VehicleId = request.VehicleId;
        loadRequest.Verified = request.Verified;
        loadRequest.UpdatedAt = DateTime.UtcNow;

        loadRequest.LoadRequestLines = request.LoadRequestLines.Select(l => new LoadRequestLine
        {
            Id = l.Id,
            LoadRequestId = loadRequest.Id,
            ProductVariantId = l.ProductVariantId,
            Quantity = l.Quantity
        }).ToList();

        await _unitOfWork.LoadRequests.UpdateAsync(loadRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
