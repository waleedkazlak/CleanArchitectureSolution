using CleanSample.Application.DTOs;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequestPart;

/// <summary>
/// Query to get parts summary (LoadRequestId, ProductId, PartId, RequiredQuantity) by LoadRequestId
/// </summary>
public class GetLoadRequestPartsByLoadRequestIdQuery : IRequest<IEnumerable<LoadRequestPartSummaryDto>>
{
    public long LoadRequestId { get; set; }

    public GetLoadRequestPartsByLoadRequestIdQuery(long loadRequestId)
    {
        LoadRequestId = loadRequestId;
    }
}
