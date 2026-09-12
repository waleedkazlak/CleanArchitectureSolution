namespace CleanSample.Domain.Enums;

/// <summary>
/// Status enumeration for Load Requests
/// </summary>
public enum LoadRequestStatusEnum
{
    New = 1,
    Loaded = 2,
    Offloaded = 3,
    Completed = 4,
    Blocked = 5,
    Cancelled = 6
}
