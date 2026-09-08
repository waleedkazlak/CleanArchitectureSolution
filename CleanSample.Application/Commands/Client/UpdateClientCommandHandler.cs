using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Client;

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(request.Id);
        if (client == null)
        {
            return false;
        }

        client.Code = request.Code;
        client.Name = request.Name;
        client.Phone = request.Phone;
        client.Mobile = request.Mobile;
        client.Email = request.Email;
        client.Address = request.Address;
        client.City = request.City;
        client.IsActive = request.IsActive;
        client.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Clients.UpdateAsync(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
