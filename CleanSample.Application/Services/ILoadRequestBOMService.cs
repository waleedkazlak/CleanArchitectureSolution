namespace CleanSample.Application.Services;

/// <summary>
/// Service for validating load requests against order requirements and generating BOM parts
/// </summary>
public interface ILoadRequestBOMService
{
    /// <summary>
    /// Checks if all load requests for the given orderId are verified and their line quantities
    /// exactly match the order line quantities. If satisfied, generates and inserts LoadRequestParts
    /// for all load request lines based on ProductBOM.
    /// </summary>
    /// <param name="orderId">The ID of the order to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if conditions were met and parts generated; otherwise false.</returns>
    Task<bool> CheckAndGeneratePartsForOrderAsync(long orderId, CancellationToken cancellationToken = default);
}
