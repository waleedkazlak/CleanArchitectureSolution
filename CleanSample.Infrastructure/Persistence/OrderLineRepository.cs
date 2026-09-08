using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class OrderLineRepository : IOrderLineRepository
{
    private readonly CleanSampleDbContext _context;

    public OrderLineRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<OrderLine?> GetByIdAsync(long id)
    {
        return await _context.OrderLines
            .Include(ol => ol.Order)
            .Include(ol => ol.ProductVariant)
            .FirstOrDefaultAsync(ol => ol.Id == id);
    }

    public async Task<IEnumerable<OrderLine>> GetAllAsync()
    {
        return await _context.OrderLines
            .Include(ol => ol.Order)
            .Include(ol => ol.ProductVariant)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderLine>> GetByOrderIdAsync(long orderId)
    {
        return await _context.OrderLines
            .Include(ol => ol.Order)
            .Include(ol => ol.ProductVariant)
            .Where(ol => ol.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(OrderLine orderLine)
    {
        _context.OrderLines.Add(orderLine);
        await _context.SaveChangesAsync();
        return orderLine.Id;
    }

    public async Task UpdateAsync(OrderLine orderLine)
    {
        _context.Entry(orderLine).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var orderLine = await _context.OrderLines.FindAsync(id);
        if (orderLine != null)
        {
            _context.OrderLines.Remove(orderLine);
            await _context.SaveChangesAsync();
        }
    }
}
