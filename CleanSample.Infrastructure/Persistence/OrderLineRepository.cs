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
            .Include(ol => ol.Product)
            .FirstOrDefaultAsync(ol => ol.Id == id);
    }

    public async Task<IEnumerable<OrderLine>> GetAllAsync()
    {
        return await _context.OrderLines
            .Include(ol => ol.Order)
            .Include(ol => ol.Product)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderLine>> GetByOrderIdAsync(long orderId)
    {
        return await _context.OrderLines
            .Include(ol => ol.Order)
            .Include(ol => ol.Product)
            .Where(ol => ol.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<OrderLine?> GetByOrderAndProductIdAsync(long orderId, int productId)
    {
        return await _context.OrderLines
            .Include(ol => ol.Order)
            .Include(ol => ol.Product)
            .FirstOrDefaultAsync(ol => ol.OrderId == orderId && ol.ProductId == productId);
    }

    public async Task<long> AddAsync(OrderLine orderLine)
    {
        _context.OrderLines.Add(orderLine);
        await _context.SaveChangesAsync();
        return orderLine.Id;
    }

    public async Task<List<OrderLine>> AddRangeAsync(IEnumerable<OrderLine> orderLines)
    {
        var lineList = orderLines.ToList();
        _context.OrderLines.AddRange(lineList);
        await _context.SaveChangesAsync();
        return lineList;
    }

    public async Task UpdateAsync(OrderLine orderLine)
    {
        _context.Entry(orderLine).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task<List<OrderLine>> UpdateRangeAsync(IEnumerable<OrderLine> orderLines)
    {
        var lineList = orderLines.ToList();
        foreach (var line in lineList)
        {
            _context.Entry(line).State = EntityState.Modified;
        }
        await _context.SaveChangesAsync();
        return lineList;
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

    public async Task<bool> DeleteRangeAsync(IEnumerable<long> ids)
    {
        var idList = ids.ToList();
        var lines = await _context.OrderLines.Where(ol => idList.Contains(ol.Id)).ToListAsync();
        if (lines.Any())
        {
            _context.OrderLines.RemoveRange(lines);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
