using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IVehicleLoadRepository
{
    Task<VehicleLoad?> GetByIdAsync(long id);
    Task<IEnumerable<VehicleLoad>> GetAllAsync();
    Task<IEnumerable<VehicleLoad>> GetByPickRequestIdAsync(long pickRequestId);
    Task<IEnumerable<VehicleLoad>> GetByDriverIdAsync(int driverId);
    Task<IEnumerable<VehicleLoad>> GetByVehicleIdAsync(int vehicleId);
    Task<long> AddAsync(VehicleLoad vehicleLoad);
    Task UpdateAsync(VehicleLoad vehicleLoad);
    Task DeleteAsync(long id);
}
