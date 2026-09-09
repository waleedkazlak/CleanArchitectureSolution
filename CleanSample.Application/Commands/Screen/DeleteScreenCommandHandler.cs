using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Screen;

public class DeleteScreenCommandHandler : IRequestHandler<DeleteScreenCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteScreenCommandHandler> _logger;

    public DeleteScreenCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteScreenCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteScreenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting Screen ID: {ScreenId}", request.Id);

        var screen = await _unitOfWork.Screens.GetByIdAsync(request.Id, cancellationToken);
        if (screen == null)
        {
            throw new KeyNotFoundException($"Screen with ID {request.Id} not found.");
        }

        await _unitOfWork.Screens.DeleteAsync(screen, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Screen ID: {ScreenId} deleted successfully", request.Id);
        return true;
    }
}
