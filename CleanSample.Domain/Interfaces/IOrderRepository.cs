namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(long id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByOrderNumberAsync(string orderNumber);
    Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);
    Task<long> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(long id);
}
