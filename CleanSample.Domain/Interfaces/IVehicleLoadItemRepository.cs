using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IVehicleLoadItemRepository
{
    Task<VehicleLoadItem?> GetByIdAsync(long id);
    Task<IEnumerable<VehicleLoadItem>> GetAllAsync();
    Task<IEnumerable<VehicleLoadItem>> GetByVehicleLoadIdAsync(long vehicleLoadId);
    Task<long> AddAsync(VehicleLoadItem vehicleLoadItem);
    Task<IEnumerable<long>> AddRangeAsync(IEnumerable<VehicleLoadItem> items);
    Task UpdateAsync(VehicleLoadItem vehicleLoadItem);
    Task DeleteAsync(long id);
    Task DeleteRangeAsync(IEnumerable<long> ids);
}
