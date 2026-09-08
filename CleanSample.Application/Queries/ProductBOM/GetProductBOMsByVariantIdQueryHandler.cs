using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.ProductBOM;

public class GetProductBOMsByVariantIdQueryHandler : IRequestHandler<GetProductBOMsByVariantIdQuery, IEnumerable<ProductBOMDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductBOMsByVariantIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductBOMDto>> Handle(GetProductBOMsByVariantIdQuery request, CancellationToken cancellationToken)
    {
        var boms = await _unitOfWork.ProductBOMs.GetByProductVariantIdAsync(request.ProductVariantId);

        return boms.Select(pb => new ProductBOMDto
        {
            Id = pb.Id,
            ProductVariantId = pb.ProductVariantId,
            ProductVariantCode = pb.ProductVariant?.Code,
            PartId = pb.PartId,
            PartCode = pb.Part?.Code,
            PartName = pb.Part?.Name,
            Quantity = pb.Quantity,
            CreatedAt = pb.CreatedAt,
            UpdatedAt = pb.UpdatedAt
        }).ToList();
    }
}
