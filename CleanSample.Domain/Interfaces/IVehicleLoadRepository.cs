using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IVehicleLoadRepository
{
    Task<VehicleLoad?> GetByIdAsync(long id);
    Task<IEnumerable<VehicleLoad>> GetAllAsync();
    Task<IEnumerable<VehicleLoad>> GetByLoadRequestIdAsync(long loadRequestId);
    Task<IEnumerable<VehicleLoad>> GetByDriverIdAsync(int driverId);
    Task<IEnumerable<VehicleLoad>> GetByBarcodeAsync(string barcode);
    Task<long> AddAsync(VehicleLoad vehicleLoad);
    Task<List<VehicleLoad>> AddRangeAsync(IEnumerable<VehicleLoad> vehicleLoads);
    Task UpdateAsync(VehicleLoad vehicleLoad);
    Task<List<VehicleLoad>> UpdateRangeAsync(IEnumerable<VehicleLoad> vehicleLoads);
    Task DeleteAsync(long id);
    Task<bool> DeleteRangeAsync(IEnumerable<long> ids);
}
