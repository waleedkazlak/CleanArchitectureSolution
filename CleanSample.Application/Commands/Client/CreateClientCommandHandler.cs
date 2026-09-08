using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Client;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var client = new CleanSample.Domain.Entities.Client
        {
            Code = request.Code,
            Name = request.Name,
            Phone = request.Phone,
            Mobile = request.Mobile,
            Email = request.Email,
            Address = request.Address,
            City = request.City,
            IsActive = request.IsActive
        };

        var clientId = await _unitOfWork.Clients.AddAsync(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return clientId;
    }
}
