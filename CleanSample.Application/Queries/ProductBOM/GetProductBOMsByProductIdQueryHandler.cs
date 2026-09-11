using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.ProductBOM;

public class GetProductBOMsByProductIdQueryHandler : IRequestHandler<GetProductBOMsByProductIdQuery, IEnumerable<ProductBOMDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductBOMsByProductIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductBOMDto>> Handle(GetProductBOMsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var boms = await _unitOfWork.ProductBOMs.GetByProductIdAsync(request.ProductId);

        return boms.Select(pb => new ProductBOMDto
        {
            Id = pb.Id,
            ProductId = pb.ProductId,
            ProductName = pb.Product?.Name,
            PartId = pb.PartId,
            PartCode = pb.Part?.Code,
            PartName = pb.Part?.Name,
            Quantity = pb.Quantity,
            CreatedAt = pb.CreatedAt,
            UpdatedAt = pb.UpdatedAt
        }).ToList();
    }
}
