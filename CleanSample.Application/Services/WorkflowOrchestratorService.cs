using CleanSample.Domain.Entities;
using CleanSample.Domain.Enums;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanSample.Application.Services;

public class WorkflowOrchestratorService : IWorkflowOrchestratorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkflowOrchestratorService> _logger;

    public WorkflowOrchestratorService(IUnitOfWork unitOfWork, ILogger<WorkflowOrchestratorService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task FlagOrderToProcessingOnLoadRequestAddedAsync(long orderId, CancellationToken cancellationToken = default)
    {
        if (orderId <= 0) return;

        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null) return;

        if (order.Status != (int)OrderStatusEnum.Processing && order.Status != (int)OrderStatusEnum.Completed)
        {
            _logger.LogInformation("Load request added for Order {OrderId}. Flagging Order.Status to Processing (4)", orderId);
            order.Status = (int)OrderStatusEnum.Processing;
            order.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RecalculateOrderStatusAfterLoadRequestChangeAsync(long orderId, CancellationToken cancellationToken = default)
    {
        if (orderId <= 0) return;

        var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
        if (order == null) return;

        var loadRequests = (await _unitOfWork.LoadRequests.GetByOrderIdAsync(orderId)).ToList();
        if (!loadRequests.Any()) return;

        bool allCompleted = loadRequests.All(lr => lr.Status == (int)LoadRequestStatusEnum.Completed);

        if (allCompleted)
        {
            if (order.Status != (int)OrderStatusEnum.Completed)
            {
                _logger.LogInformation("All load requests for Order {OrderId} are completed. Updating Order.Status to Completed (5)", orderId);
                order.Status = (int)OrderStatusEnum.Completed;
                order.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
        else
        {
            // When any load request exists for that order, flag order status to Processing (4)
            if (order.Status != (int)OrderStatusEnum.Processing && order.Status != (int)OrderStatusEnum.Completed)
            {
                _logger.LogInformation("Load request exists for Order {OrderId}. Flagging Order.Status to Processing (4)", orderId);
                order.Status = (int)OrderStatusEnum.Processing;
                order.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Orders.UpdateAsync(order);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }

    public async Task RecalculateLoadRequestStatusAfterVehicleLoadsChangeAsync(long loadRequestId, CancellationToken cancellationToken = default)
    {
        if (loadRequestId <= 0) return;

        var loadRequest = await _unitOfWork.LoadRequests.GetByIdAsync(loadRequestId);
        if (loadRequest == null) return;

        var vehicleLoads = (await _unitOfWork.VehicleLoads.GetByLoadRequestIdAsync(loadRequestId)).ToList();
        if (!vehicleLoads.Any()) return;

        // Check if ANY vehicle load is Damaged or Missing
        bool hasIssue = vehicleLoads.Any(vl =>
            vl.Status == (int)VehicleLoadStatusEnum.Damaged ||
            vl.Status == (int)VehicleLoadStatusEnum.Missing);

        if (hasIssue)
        {
            _logger.LogWarning("LoadRequest {LoadRequestId} has Damaged/Missing vehicle loads. Updating status to Blocked (5)", loadRequestId);
            loadRequest.Status = (int)LoadRequestStatusEnum.Blocked;
            loadRequest.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.LoadRequests.UpdateAsync(loadRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        // Check if all vehicle loads are Good
        bool allGood = vehicleLoads.All(vl => vl.Status == (int)VehicleLoadStatusEnum.Good);
        if (allGood && (loadRequest.Status == (int)LoadRequestStatusEnum.New || loadRequest.Status == (int)LoadRequestStatusEnum.Blocked))
        {
            _logger.LogInformation("All vehicle loads for LoadRequest {LoadRequestId} are Good. Updating status to Loaded (2)", loadRequestId);
            loadRequest.Status = (int)LoadRequestStatusEnum.Loaded;
            loadRequest.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.LoadRequests.UpdateAsync(loadRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RecalculateLoadRequestStatusAfterVehicleOffloadsChangeAsync(long loadRequestId, CancellationToken cancellationToken = default)
    {
        if (loadRequestId <= 0) return;

        var loadRequest = await _unitOfWork.LoadRequests.GetByIdAsync(loadRequestId);
        if (loadRequest == null) return;

        var vehicleOffloads = (await _unitOfWork.VehicleOffloads.GetByLoadRequestIdAsync(loadRequestId)).ToList();
        var vehicleLoads = (await _unitOfWork.VehicleLoads.GetByLoadRequestIdAsync(loadRequestId)).ToList();

        // Check if ANY vehicle offload or vehicle load is Damaged or Missing
        bool hasOffloadIssue = vehicleOffloads.Any(vo =>
            vo.Status == (int)VehicleOffloadStatusEnum.Damaged ||
            vo.Status == (int)VehicleOffloadStatusEnum.Missing);

        bool hasLoadIssue = vehicleLoads.Any(vl =>
            vl.Status == (int)VehicleLoadStatusEnum.Damaged ||
            vl.Status == (int)VehicleLoadStatusEnum.Missing);

        if (hasOffloadIssue || hasLoadIssue)
        {
            _logger.LogWarning("LoadRequest {LoadRequestId} has Damaged/Missing vehicle offloads or loads. Updating status to Blocked (5)", loadRequestId);
            loadRequest.Status = (int)LoadRequestStatusEnum.Blocked;
            loadRequest.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.LoadRequests.UpdateAsync(loadRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;
        }

        // If all offloads are Good (and all loads are Good)
        if (vehicleOffloads.Any() && vehicleOffloads.All(vo => vo.Status == (int)VehicleOffloadStatusEnum.Good))
        {
            _logger.LogInformation("All vehicle offloads for LoadRequest {LoadRequestId} are Good. Updating status to Offloaded (3)", loadRequestId);
            loadRequest.Status = (int)LoadRequestStatusEnum.Offloaded;
            loadRequest.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.LoadRequests.UpdateAsync(loadRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RecalculateLoadRequestStatusAfterFieldAssembliesChangeAsync(long loadRequestId, CancellationToken cancellationToken = default)
    {
        if (loadRequestId <= 0) return;

        var loadRequest = await _unitOfWork.LoadRequests.GetByIdAsync(loadRequestId);
        if (loadRequest == null) return;

        var fieldJobs = (await _unitOfWork.FieldJobs.GetByLoadRequestIdAsync(loadRequestId)).ToList();
        if (!fieldJobs.Any()) return;

        var allAssemblies = new List<FieldAssembly>();
        foreach (var job in fieldJobs)
        {
            var assemblies = await _unitOfWork.FieldAssemblies.GetByFieldJobIdAsync(job.Id);
            allAssemblies.AddRange(assemblies);
        }

        if (!allAssemblies.Any()) return;

        // Check if all assemblies related to the load request are Completed (2)
        bool allCompleted = allAssemblies.All(fa => fa.Status == (int)FieldAssemblyStatusEnum.Completed);

        if (allCompleted)
        {
            _logger.LogInformation("All field assemblies for LoadRequest {LoadRequestId} are Completed. Updating LoadRequest.Status to Completed (4)", loadRequestId);
            loadRequest.Status = (int)LoadRequestStatusEnum.Completed;
            loadRequest.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.LoadRequests.UpdateAsync(loadRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Cascade check to parent order if associated
            if (loadRequest.OrderId.HasValue)
            {
                await RecalculateOrderStatusAfterLoadRequestChangeAsync(loadRequest.OrderId.Value, cancellationToken);
            }
        }
    }
}
