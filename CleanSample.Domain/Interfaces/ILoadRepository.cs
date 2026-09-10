using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface ILoadRepository
{
    Task<Load?> GetByIdAsync(long id);
    Task<IEnumerable<Load>> GetAllAsync();
    Task<IEnumerable<Load>> GetByLoadRequestIdAsync(long loadRequestId);
    Task<IEnumerable<Load>> GetByDriverIdAsync(int driverId);
    Task<IEnumerable<Load>> GetByBarcodeAsync(string barcode);
    Task<long> AddAsync(Load load);
    Task<List<Load>> AddRangeAsync(IEnumerable<Load> loads);
    Task UpdateAsync(Load load);
    Task<List<Load>> UpdateRangeAsync(IEnumerable<Load> loads);
    Task DeleteAsync(long id);
    Task<bool> DeleteRangeAsync(IEnumerable<long> ids);
}
