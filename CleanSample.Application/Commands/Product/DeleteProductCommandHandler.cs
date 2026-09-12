using CleanSample.Application.Services;
using CleanSample.Domain.Interfaces;
using MediatR;

namespace CleanSample.Application.Commands.Product;
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IFileStorageService fileStorageService)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);
        if (product == null)
            return false;

        if (!string.IsNullOrWhiteSpace(product.PictureUrl))
        {
            await _fileStorageService.DeleteFileAsync(product.PictureUrl, cancellationToken);
        }

        await _unitOfWork.Products.DeleteAsync(product.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
