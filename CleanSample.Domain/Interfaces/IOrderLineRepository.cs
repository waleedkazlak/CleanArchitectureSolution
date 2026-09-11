namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IOrderLineRepository
{
    Task<OrderLine?> GetByIdAsync(long id);
    Task<IEnumerable<OrderLine>> GetAllAsync();
    Task<IEnumerable<OrderLine>> GetByOrderIdAsync(long orderId);
    Task<OrderLine?> GetByOrderAndProductIdAsync(long orderId, int productId);
    Task<long> AddAsync(OrderLine orderLine);
    Task<List<OrderLine>> AddRangeAsync(IEnumerable<OrderLine> orderLines);
    Task UpdateAsync(OrderLine orderLine);
    Task<List<OrderLine>> UpdateRangeAsync(IEnumerable<OrderLine> orderLines);
    Task DeleteAsync(long id);
    Task<bool> DeleteRangeAsync(IEnumerable<long> ids);
}
