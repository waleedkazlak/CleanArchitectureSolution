using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequestLine;

public class GetApprovedLoadRequestLinesByDriverIdQuery : IRequest<PaginatedResultDto<LoadRequestLineDto>>
{
    public int DriverId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; } = "CreatedAt";
    public string? SortDirection { get; set; } = "desc";

    public GetApprovedLoadRequestLinesByDriverIdQuery(
        int driverId,
        int pageNumber = 1,
        int pageSize = 10,
        string? searchTerm = null,
        string? sortBy = "CreatedAt",
        string? sortDirection = "desc")
    {
        DriverId = driverId;
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchTerm = searchTerm;
        SortBy = sortBy;
        SortDirection = sortDirection;
    }
}
