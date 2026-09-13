using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Commands.Screen;

public class CreateScreenCommandHandler : IRequestHandler<CreateScreenCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateScreenCommandHandler> _logger;

    public CreateScreenCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateScreenCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<int> Handle(CreateScreenCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new Screen with Code: {Code}", request.Code);

        var existingCode = await _unitOfWork.Screens.GetByCodeAsync(request.Code, cancellationToken);
        if (existingCode != null)
        {
            throw new InvalidOperationException($"Screen with code '{request.Code}' already exists.");
        }

        var nameEn = !string.IsNullOrWhiteSpace(request.NameEn) ? request.NameEn : (request.Name ?? string.Empty);
        var nameAr = request.NameAr;
        var descEn = !string.IsNullOrWhiteSpace(request.DescriptionEn) ? request.DescriptionEn : request.Description;
        var descAr = request.DescriptionAr;

        var existingName = await _unitOfWork.Screens.GetByNameAsync(nameEn, cancellationToken);
        if (existingName != null)
        {
            throw new InvalidOperationException($"Screen with name '{nameEn}' already exists.");
        }

        var screen = new Domain.Entities.Screen
        {
            NameEn = nameEn,
            NameAr = nameAr,
            Code = request.Code.ToUpperInvariant(),
            Module = request.Module,
            DescriptionEn = descEn,
            DescriptionAr = descAr,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Screens.AddAsync(screen, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Screen created successfully with ID: {ScreenId}", screen.Id);
        return screen.Id;
    }
}
