using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.PickRequest;

public class GetPickRequestByIdQuery : IRequest<PickRequestDto?>
{
    public long Id { get; set; }

    public GetPickRequestByIdQuery(long id)
    {
        Id = id;
    }
}
