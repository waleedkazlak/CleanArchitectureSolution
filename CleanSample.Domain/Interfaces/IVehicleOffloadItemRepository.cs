using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IVehicleOffloadItemRepository
{
    Task<VehicleOffloadItem?> GetByIdAsync(long id);
    Task<IEnumerable<VehicleOffloadItem>> GetAllAsync();
    Task<IEnumerable<VehicleOffloadItem>> GetByVehicleOffloadIdAsync(long vehicleOffloadId);
    Task<IEnumerable<VehicleOffloadItem>> GetByLoadIdAsync(long loadId);
    Task<IEnumerable<VehicleOffloadItem>> GetByBarcodeAsync(string barcode);
    Task<long> AddAsync(VehicleOffloadItem item);
    Task<List<VehicleOffloadItem>> AddRangeAsync(IEnumerable<VehicleOffloadItem> items);
    Task UpdateAsync(VehicleOffloadItem item);
    Task DeleteAsync(long id);
}
