using CleanSample.Application.DTOs;
using CleanSample.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Queries.Screen;

public class GetAllScreensQueryHandler : IRequestHandler<GetAllScreensQuery, IReadOnlyList<ScreenDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllScreensQueryHandler> _logger;

    public GetAllScreensQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllScreensQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IReadOnlyList<ScreenDto>> Handle(GetAllScreensQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all screens (OnlyActive: {OnlyActive})", request.OnlyActive);

        var screens = request.OnlyActive
            ? await _unitOfWork.Screens.GetActiveAsync(cancellationToken)
            : await _unitOfWork.Screens.GetAllAsync(cancellationToken);

        return screens.Select(s => new ScreenDto
        {
            ScreenId = s.Id,
            Name = s.Name,
            Code = s.Code,
            Module = s.Module,
            Description = s.Description,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();
    }
}
