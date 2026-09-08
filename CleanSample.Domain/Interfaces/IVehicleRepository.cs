namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByIdAsync(int id);
    Task<IEnumerable<Vehicle>> GetAllAsync();
    Task<int> AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task DeleteAsync(int id);
}
