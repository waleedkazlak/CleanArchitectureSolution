using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.LoadRequestLine;

public class DeleteLoadRequestLineCommandHandler : IRequestHandler<DeleteLoadRequestLineCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLoadRequestLineCommandHandler> _logger;

    public DeleteLoadRequestLineCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteLoadRequestLineCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteLoadRequestLineCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting LoadRequestLine with Id: {Id}", request.Id);

        var existing = await _unitOfWork.LoadRequestLines.GetByIdAsync(request.Id);
        if (existing == null)
        {
            _logger.LogWarning("LoadRequestLine with Id: {Id} not found", request.Id);
            return false;
        }

        await _unitOfWork.LoadRequestLines.DeleteAsync(request.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("LoadRequestLine with Id: {Id} deleted successfully", request.Id);
        return true;
    }
}
