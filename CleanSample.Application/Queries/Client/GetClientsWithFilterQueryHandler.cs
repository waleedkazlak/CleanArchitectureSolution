using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Client;

public class GetClientsWithFilterQueryHandler : IRequestHandler<GetClientsWithFilterQuery, PaginatedResultDto<ClientDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetClientsWithFilterQueryHandler> _logger;

    public GetClientsWithFilterQueryHandler(IUnitOfWork unitOfWork, ILogger<GetClientsWithFilterQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaginatedResultDto<ClientDto>> Handle(GetClientsWithFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;

        _logger.LogInformation(
            "Handling GetClientsWithFilterQuery - Page: {PageNumber}, PageSize: {PageSize}, Search: {SearchTerm}",
            filter.PageNumber, filter.PageSize, filter.SearchTerm);

        var pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
        var pageSize = filter.PageSize > 0 && filter.PageSize <= 100 ? filter.PageSize : 10;

        var clients = await _unitOfWork.Clients.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var searchTermLower = filter.SearchTerm.ToLower();
            clients = clients.Where(c =>
                (c.Code != null && c.Code.ToLower().Contains(searchTermLower)) ||
                c.Name.ToLower().Contains(searchTermLower) ||
                (c.Phone != null && c.Phone.ToLower().Contains(searchTermLower)) ||
                (c.Mobile != null && c.Mobile.ToLower().Contains(searchTermLower)) ||
                (c.Email != null && c.Email.ToLower().Contains(searchTermLower)) ||
                (c.Address != null && c.Address.ToLower().Contains(searchTermLower)) ||
                (c.City != null && c.City.ToLower().Contains(searchTermLower))
            ).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Code))
        {
            var codeLower = filter.Code.ToLower();
            clients = clients.Where(c => c.Code != null && c.Code.ToLower().Contains(codeLower)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var nameLower = filter.Name.ToLower();
            clients = clients.Where(c => c.Name.ToLower().Contains(nameLower)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Phone))
        {
            var phoneLower = filter.Phone.ToLower();
            clients = clients.Where(c => (c.Phone != null && c.Phone.ToLower().Contains(phoneLower)) ||
                                         (c.Mobile != null && c.Mobile.ToLower().Contains(phoneLower))).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            var emailLower = filter.Email.ToLower();
            clients = clients.Where(c => c.Email != null && c.Email.ToLower().Contains(emailLower)).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            var cityLower = filter.City.ToLower();
            clients = clients.Where(c => c.City != null && c.City.ToLower().Contains(cityLower)).ToList();
        }

        if (filter.IsActive.HasValue)
        {
            clients = clients.Where(c => c.IsActive == filter.IsActive.Value).ToList();
        }

        clients = ApplySort(clients.ToList(), filter.SortBy, filter.SortDirection);

        var totalCount = clients.Count();

        var paginatedClients = clients
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var dtos = paginatedClients.Select(c => new ClientDto
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            Phone = c.Phone,
            Mobile = c.Mobile,
            Email = c.Email,
            Address = c.Address,
            City = c.City,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        }).ToList();

        return new PaginatedResultDto<ClientDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    private List<Domain.Entities.Client> ApplySort(List<Domain.Entities.Client> clients, string? sortBy, string? sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";

        return (sortBy?.ToLower()) switch
        {
            "code" => isDescending
                ? clients.OrderByDescending(c => c.Code).ToList()
                : clients.OrderBy(c => c.Code).ToList(),

            "name" => isDescending
                ? clients.OrderByDescending(c => c.Name).ToList()
                : clients.OrderBy(c => c.Name).ToList(),

            "city" => isDescending
                ? clients.OrderByDescending(c => c.City).ToList()
                : clients.OrderBy(c => c.City).ToList(),

            "email" => isDescending
                ? clients.OrderByDescending(c => c.Email).ToList()
                : clients.OrderBy(c => c.Email).ToList(),

            "createdat" => isDescending
                ? clients.OrderByDescending(c => c.CreatedAt).ToList()
                : clients.OrderBy(c => c.CreatedAt).ToList(),

            _ => clients.OrderByDescending(c => c.CreatedAt).ToList()
        };
    }
}
