using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.LoadRequestPart;

public class GetLoadRequestPartsByLoadRequestIdQueryHandler : IRequestHandler<GetLoadRequestPartsByLoadRequestIdQuery, IEnumerable<LoadRequestPartSummaryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetLoadRequestPartsByLoadRequestIdQueryHandler> _logger;

    public GetLoadRequestPartsByLoadRequestIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetLoadRequestPartsByLoadRequestIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<LoadRequestPartSummaryDto>> Handle(
        GetLoadRequestPartsByLoadRequestIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetLoadRequestPartsByLoadRequestIdQuery for LoadRequestId: {LoadRequestId}", request.LoadRequestId);

        var parts = await _unitOfWork.LoadRequestParts.GetByLoadRequestIdAsync(request.LoadRequestId);

        return parts.Select(p => new LoadRequestPartSummaryDto
        {
            LoadRequestId = p.LoadRequestId,
            ProductId = p.ProductId,
            PartId = p.PartId,
            RequiredQuantity = p.RequiredQuantity
        }).ToList();
    }
}
