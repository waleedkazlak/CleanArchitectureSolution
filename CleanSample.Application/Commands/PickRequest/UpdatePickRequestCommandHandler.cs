using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.PickRequest;

public class UpdatePickRequestCommandHandler : IRequestHandler<UpdatePickRequestCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePickRequestCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdatePickRequestCommand request, CancellationToken cancellationToken)
    {
        var pickRequest = await _unitOfWork.PickRequests.GetByIdAsync(request.Id);
        if (pickRequest == null)
        {
            return false;
        }

        pickRequest.RequestNumber = request.RequestNumber;
        pickRequest.OrderId = request.OrderId;
        pickRequest.ClientId = request.ClientId;
        pickRequest.ClientLocationId = request.ClientLocationId;
        pickRequest.RequestedBy = request.RequestedBy;
        pickRequest.RequestDate = request.RequestDate;
        pickRequest.ExecutionDate = request.ExecutionDate;
        pickRequest.Status = request.Status;
        pickRequest.DestinationAddress = request.DestinationAddress;
        pickRequest.DestinationCity = request.DestinationCity;
        pickRequest.Description = request.Description;
        pickRequest.DriverId = request.DriverId;
        pickRequest.VehicleId = request.VehicleId;
        pickRequest.Verified = request.Verified;
        pickRequest.UpdatedAt = DateTime.UtcNow;

        pickRequest.PickRequestLines = request.PickRequestLines.Select(l => new PickRequestLine
        {
            Id = l.Id,
            PickRequestId = pickRequest.Id,
            ProductVariantId = l.ProductVariantId,
            Quantity = l.Quantity
        }).ToList();

        await _unitOfWork.PickRequests.UpdateAsync(pickRequest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
