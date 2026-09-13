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

        var nameEn = !string.IsNullOrWhiteSpace(request.NameEn) ? request.NameEn : (request.Name ?? string.Empty);
        var nameAr = request.NameAr;
        var descEn = !string.IsNullOrWhiteSpace(request.DescriptionEn) ? request.DescriptionEn : request.Description;
        var descAr = request.DescriptionAr;

        if (!string.IsNullOrWhiteSpace(nameEn))
        {
            var existingName = await _unitOfWork.Screens.GetByNameAsync(nameEn, cancellationToken);
            if (existingName != null && existingName.Id != request.Id)
            {
                throw new InvalidOperationException($"Screen with name '{nameEn}' already exists.");
            }
            screen.NameEn = nameEn;
        }

        if (nameAr != null)
        {
            screen.NameAr = nameAr;
        }

        screen.Code = request.Code.ToUpperInvariant();
        screen.Module = request.Module;

        if (!string.IsNullOrWhiteSpace(descEn))
            screen.DescriptionEn = descEn;
        else if (request.Description != null)
            screen.DescriptionEn = request.Description;

        if (descAr != null)
            screen.DescriptionAr = descAr;

        screen.IsActive = request.IsActive;
        screen.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Screens.UpdateAsync(screen, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Screen ID: {ScreenId} updated successfully", request.Id);
        return true;
    }
}
