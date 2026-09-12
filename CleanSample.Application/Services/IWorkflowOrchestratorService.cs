namespace CleanSample.Application.Services;

public interface IWorkflowOrchestratorService
{
    /// <summary>
    /// Checks all load requests for an order. If all load requests are Completed (4), transitions Order.Status to Completed (5).
    /// If there are active load requests and order is not Completed, ensures Order.Status is Processing (4).
    /// </summary>
    Task RecalculateOrderStatusAfterLoadRequestChangeAsync(long orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Flags the specified order to Processing (4) when a load request is added for that order.
    /// </summary>
    Task FlagOrderToProcessingOnLoadRequestAddedAsync(long orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks all vehicle loads for a load request:
    /// - If ANY vehicle load is Damaged (2) or Missing (3), sets LoadRequest.Status to Blocked (5).
    /// - If ALL vehicle loads are Good (1), sets LoadRequest.Status to Loaded (2).
    /// </summary>
    Task RecalculateLoadRequestStatusAfterVehicleLoadsChangeAsync(long loadRequestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks all vehicle offloads for a load request:
    /// - If ANY vehicle offload is Damaged (2) or Missing (3), sets LoadRequest.Status to Blocked (5).
    /// - If ALL vehicle loads and offloads are Good (1), sets LoadRequest.Status to Offloaded (3).
    /// </summary>
    Task RecalculateLoadRequestStatusAfterVehicleOffloadsChangeAsync(long loadRequestId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks all field assemblies for a load request:
    /// - If all field assemblies for all products related to the load request are Completed (2),
    ///   sets LoadRequest.Status to Completed (4) and cascades the check to the parent Order.
    /// </summary>
    Task RecalculateLoadRequestStatusAfterFieldAssembliesChangeAsync(long loadRequestId, CancellationToken cancellationToken = default);
}
