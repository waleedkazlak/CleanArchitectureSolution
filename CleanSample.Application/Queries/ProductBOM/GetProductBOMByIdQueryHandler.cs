using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Queries.ProductBOM;

public class GetProductBOMByIdQueryHandler : IRequestHandler<GetProductBOMByIdQuery, ProductBOMDto?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProductBOMByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductBOMDto?> Handle(GetProductBOMByIdQuery request, CancellationToken cancellationToken)
    {
        var pb = await _unitOfWork.ProductBOMs.GetByIdAsync(request.Id);
        if (pb == null)
        {
            return null;
        }

        return new ProductBOMDto
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
        };
    }
}
