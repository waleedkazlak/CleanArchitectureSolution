using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Screen;

public class GetScreenByIdQueryHandler : IRequestHandler<GetScreenByIdQuery, ScreenDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetScreenByIdQueryHandler> _logger;

    public GetScreenByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetScreenByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ScreenDto?> Handle(GetScreenByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting Screen by ID: {ScreenId}", request.Id);

        var screen = await _unitOfWork.Screens.GetByIdAsync(request.Id, cancellationToken);
        if (screen == null) return null;

        return new ScreenDto
        {
            ScreenId = screen.Id,
            Name = screen.Name,
            Code = screen.Code,
            Module = screen.Module,
            Description = screen.Description,
            IsActive = screen.IsActive,
            CreatedAt = screen.CreatedAt,
            UpdatedAt = screen.UpdatedAt
        };
    }
}
