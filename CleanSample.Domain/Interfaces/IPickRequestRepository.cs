using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IPickRequestRepository
{
    Task<PickRequest?> GetByIdAsync(long id);
    Task<IEnumerable<PickRequest>> GetAllAsync();
    Task<PickRequest?> GetByRequestNumberAsync(string requestNumber);
    Task<IEnumerable<PickRequest>> GetByOrderIdAsync(long orderId);
    Task<IEnumerable<PickRequest>> GetByClientIdAsync(int clientId);
    Task<long> AddAsync(PickRequest pickRequest);
    Task UpdateAsync(PickRequest pickRequest);
    Task DeleteAsync(long id);
}
