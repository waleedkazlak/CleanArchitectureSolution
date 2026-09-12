namespace CleanSample.Domain.Enums;

/// <summary>
/// Status enumeration for Load Requests
/// </summary>
public enum LoadRequestStatusEnum
{
    New = 1,
    Loading = 2,
    Offloaded = 3,
    Completed = 4,
    Cancelled = 5
}
