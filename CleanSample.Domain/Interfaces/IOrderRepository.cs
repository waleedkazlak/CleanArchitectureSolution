namespace CleanSample.Domain.Interfaces;

using CleanSample.Domain.Entities;

public interface IOrderRepository
{
    IQueryable<Order> GetQueryable();
    Task<Order?> GetByIdAsync(long id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByClientIdAsync(int clientId);
    Task<long> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(long id);
}
