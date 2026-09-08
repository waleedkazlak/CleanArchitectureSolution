namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IOrderLineRepository
{
    Task<OrderLine?> GetByIdAsync(long id);
    Task<IEnumerable<OrderLine>> GetAllAsync();
    Task<IEnumerable<OrderLine>> GetByOrderIdAsync(long orderId);
    Task<long> AddAsync(OrderLine orderLine);
    Task UpdateAsync(OrderLine orderLine);
    Task DeleteAsync(long id);
}
