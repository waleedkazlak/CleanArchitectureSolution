using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.Client;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetClientByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ClientDto?> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(request.Id);
        if (client == null)
        {
            return null;
        }

        return new ClientDto
        {
            Id = client.Id,
            Code = client.Code,
            Name = client.Name,
            Phone = client.Phone,
            Mobile = client.Mobile,
            Email = client.Email,
            Address = client.Address,
            City = client.City,
            IsActive = client.IsActive,
            CreatedAt = client.CreatedAt,
            UpdatedAt = client.UpdatedAt
        };
    }
}
