using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Commands.ClientLocation;

public class CreateClientLocationCommand : IRequest<List<ClientLocationDto>>
{
    public int? Id { get; set; }
    public int ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? City { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool IsDefault { get; set; } = false;
    public bool IsActive { get; set; } = true;

    public List<CreateClientLocationItemDto> Items { get; set; } = new();

    public CreateClientLocationCommand()
    {
    }

    public CreateClientLocationCommand(List<CreateClientLocationItemDto> items)
    {
        Items = items ?? new List<CreateClientLocationItemDto>();
    }

    public CreateClientLocationCommand(
        int clientId,
        string name,
        string address,
        string? city = null,
        string? contactName = null,
        string? contactPhone = null,
        decimal? latitude = null,
        decimal? longitude = null,
        bool isDefault = false,
        bool isActive = true,
        int? id = null)
    {
        ClientId = clientId;
        Name = name;
        Address = address;
        City = city;
        ContactName = contactName;
        ContactPhone = contactPhone;
        Latitude = latitude;
        Longitude = longitude;
        IsDefault = isDefault;
        IsActive = isActive;
        Id = id;
    }
}
