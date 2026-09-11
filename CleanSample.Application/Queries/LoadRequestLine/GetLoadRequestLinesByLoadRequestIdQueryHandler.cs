using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.LoadRequestLine;

public class GetLoadRequestLinesByLoadRequestIdQueryHandler : IRequestHandler<GetLoadRequestLinesByLoadRequestIdQuery, IEnumerable<LoadRequestLineDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetLoadRequestLinesByLoadRequestIdQueryHandler> _logger;

    public GetLoadRequestLinesByLoadRequestIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetLoadRequestLinesByLoadRequestIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<LoadRequestLineDto>> Handle(GetLoadRequestLinesByLoadRequestIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetLoadRequestLinesByLoadRequestIdQuery for LoadRequestId: {LoadRequestId}", request.LoadRequestId);

        var lines = await _unitOfWork.LoadRequestLines.GetByLoadRequestIdAsync(request.LoadRequestId);

        return lines.Select(line => new LoadRequestLineDto
        {
            Id = line.Id,
            LoadRequestId = line.LoadRequestId,
            ProductId = line.ProductId,
            ProductName = line.Product?.Name,
            Quantity = line.Quantity,
            CreatedAt = line.CreatedAt,
            UpdatedAt = line.UpdatedAt,
            LoadRequestParts = line.LoadRequestParts?.Select(lrp => new LoadRequestPartDto
            {
                Id = lrp.Id,
                LoadRequestId = lrp.LoadRequestId,
                LoadRequestLineId = lrp.LoadRequestLineId,
                PartId = lrp.PartId,
                PartCode = lrp.Part?.Code,
                PartName = lrp.Part?.Name,
                RequiredQuantity = lrp.RequiredQuantity,
                LoadedQuantity = lrp.LoadedQuantity,
                Status = lrp.Status,
                CreatedAt = lrp.CreatedAt,
                UpdatedAt = lrp.UpdatedAt
            }).ToList() ?? new List<LoadRequestPartDto>()
        }).ToList();
    }
}
