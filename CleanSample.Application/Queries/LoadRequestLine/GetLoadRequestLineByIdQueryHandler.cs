using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.LoadRequestLine;

public class GetLoadRequestLineByIdQueryHandler : IRequestHandler<GetLoadRequestLineByIdQuery, LoadRequestLineDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetLoadRequestLineByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LoadRequestLineDto?> Handle(GetLoadRequestLineByIdQuery request, CancellationToken cancellationToken)
    {
        var line = await _unitOfWork.LoadRequestLines.GetByIdAsync(request.Id);
        if (line == null)
        {
            return null;
        }

        return new LoadRequestLineDto
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
                ProductId = lrp.ProductId,
                ProductName = lrp.Product?.Name ?? line.Product?.Name,
                PartId = lrp.PartId,
                PartCode = lrp.Part?.Code,
                PartName = lrp.Part?.Name,
                RequiredQuantity = lrp.RequiredQuantity,
                LoadedQuantity = lrp.LoadedQuantity,
                Status = lrp.Status,
                CreatedAt = lrp.CreatedAt,
                UpdatedAt = lrp.UpdatedAt
            }).ToList() ?? new List<LoadRequestPartDto>()
        };
    }
}
