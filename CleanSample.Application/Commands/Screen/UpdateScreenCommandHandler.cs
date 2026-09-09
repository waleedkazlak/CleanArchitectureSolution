using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Screen;

public class UpdateScreenCommandHandler : IRequestHandler<UpdateScreenCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateScreenCommandHandler> _logger;

    public UpdateScreenCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateScreenCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateScreenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating Screen ID: {ScreenId}", request.Id);

        var screen = await _unitOfWork.Screens.GetByIdAsync(request.Id, cancellationToken);
        if (screen == null)
        {
            throw new KeyNotFoundException($"Screen with ID {request.Id} not found.");
        }

        var existingCode = await _unitOfWork.Screens.GetByCodeAsync(request.Code, cancellationToken);
        if (existingCode != null && existingCode.Id != request.Id)
        {
            throw new InvalidOperationException($"Screen with code '{request.Code}' already exists.");
        }

        var existingName = await _unitOfWork.Screens.GetByNameAsync(request.Name, cancellationToken);
        if (existingName != null && existingName.Id != request.Id)
        {
            throw new InvalidOperationException($"Screen with name '{request.Name}' already exists.");
        }

        screen.Name = request.Name;
        screen.Code = request.Code.ToUpperInvariant();
        screen.Module = request.Module;
        screen.Description = request.Description;
        screen.IsActive = request.IsActive;
        screen.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Screens.UpdateAsync(screen, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Screen ID: {ScreenId} updated successfully", request.Id);
        return true;
    }
}
