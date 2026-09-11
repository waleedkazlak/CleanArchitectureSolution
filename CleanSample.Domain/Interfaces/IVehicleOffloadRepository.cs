using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IVehicleOffloadRepository
{
    Task<VehicleOffload?> GetByIdAsync(long id);
    Task<IEnumerable<VehicleOffload>> GetAllAsync();
    Task<IEnumerable<VehicleOffload>> GetByLoadRequestIdAsync(long loadRequestId);
    Task<IEnumerable<VehicleOffload>> GetByVehicleIdAsync(int vehicleId);
    Task<IEnumerable<VehicleOffload>> GetByDriverIdAsync(int driverId);
    Task<long> AddAsync(VehicleOffload vehicleOffload);
    Task AddRangeAsync(IEnumerable<VehicleOffload> vehicleOffloads);
    Task UpdateAsync(VehicleOffload vehicleOffload);
    Task DeleteAsync(long id);
}
