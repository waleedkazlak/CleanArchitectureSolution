using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class OrderRepository : IOrderRepository
{
    private readonly CleanSampleDbContext _context;

    public OrderRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(long id)
    {
        return await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
            .ToListAsync();
    }


    public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId)
    {
        return await _context.Orders
            .Include(o => o.Client)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
            .Where(o => o.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order.Id;
    }

    public async Task UpdateAsync(Order order)
    {
        var existingOrder = await _context.Orders
            .Include(o => o.OrderLines)
            .FirstOrDefaultAsync(o => o.Id == order.Id);

        if (existingOrder != null)
        {
            if (existingOrder != order)
            {
                existingOrder.ClientId = order.ClientId;
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.RequiredDate = order.RequiredDate;
                existingOrder.Status = order.Status;
                existingOrder.Notes = order.Notes;
                existingOrder.UpdatedAt = DateTime.UtcNow;

                // Synchronize OrderLines
                var incomingLineIds = order.OrderLines
                    .Where(l => l.Id > 0)
                    .Select(l => l.Id)
                    .ToHashSet();

                // Remove lines not in incoming list
                var linesToRemove = existingOrder.OrderLines
                    .Where(l => !incomingLineIds.Contains(l.Id))
                    .ToList();

                foreach (var lineToRemove in linesToRemove)
                {
                    _context.OrderLines.Remove(lineToRemove);
                }

                // Update existing lines or add new lines
                var incomingLines = order.OrderLines.ToList();
                foreach (var incomingLine in incomingLines)
                {
                    if (incomingLine.Id > 0)
                    {
                        var existingLine = existingOrder.OrderLines
                            .FirstOrDefault(l => l.Id == incomingLine.Id);

                        if (existingLine != null)
                        {
                            existingLine.ProductId = incomingLine.ProductId;
                            existingLine.Quantity = incomingLine.Quantity;
                            existingLine.Notes = incomingLine.Notes;
                            existingLine.UpdatedAt = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        existingOrder.OrderLines.Add(new OrderLine
                        {
                            OrderId = existingOrder.Id,
                            ProductId = incomingLine.ProductId,
                            Quantity = incomingLine.Quantity,
                            Notes = incomingLine.Notes,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(long id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
