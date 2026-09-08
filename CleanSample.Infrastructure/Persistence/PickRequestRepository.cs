using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class PickRequestRepository : IPickRequestRepository
{
    private readonly CleanSampleDbContext _context;

    public PickRequestRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<PickRequest?> GetByIdAsync(long id)
    {
        return await _context.PickRequests
            .Include(p => p.Order)
            .Include(p => p.Client)
            .Include(p => p.ClientLocation)
            .Include(p => p.Requester)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Include(p => p.PickRequestLines)
                .ThenInclude(l => l.ProductVariant)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<PickRequest>> GetAllAsync()
    {
        return await _context.PickRequests
            .Include(p => p.Order)
            .Include(p => p.Client)
            .Include(p => p.ClientLocation)
            .Include(p => p.Requester)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Include(p => p.PickRequestLines)
                .ThenInclude(l => l.ProductVariant)
            .ToListAsync();
    }

    public async Task<PickRequest?> GetByRequestNumberAsync(string requestNumber)
    {
        return await _context.PickRequests
            .Include(p => p.Order)
            .Include(p => p.Client)
            .Include(p => p.ClientLocation)
            .Include(p => p.Requester)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Include(p => p.PickRequestLines)
                .ThenInclude(l => l.ProductVariant)
            .FirstOrDefaultAsync(p => p.RequestNumber == requestNumber);
    }

    public async Task<IEnumerable<PickRequest>> GetByOrderIdAsync(long orderId)
    {
        return await _context.PickRequests
            .Include(p => p.Order)
            .Include(p => p.Client)
            .Include(p => p.ClientLocation)
            .Include(p => p.Requester)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Include(p => p.PickRequestLines)
                .ThenInclude(l => l.ProductVariant)
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<IEnumerable<PickRequest>> GetByClientIdAsync(int clientId)
    {
        return await _context.PickRequests
            .Include(p => p.Order)
            .Include(p => p.Client)
            .Include(p => p.ClientLocation)
            .Include(p => p.Requester)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Include(p => p.PickRequestLines)
                .ThenInclude(l => l.ProductVariant)
            .Where(p => p.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(PickRequest pickRequest)
    {
        _context.PickRequests.Add(pickRequest);
        await _context.SaveChangesAsync();
        return pickRequest.Id;
    }

    public async Task UpdateAsync(PickRequest pickRequest)
    {
        var existingPickRequest = await _context.PickRequests
            .Include(p => p.PickRequestLines)
            .FirstOrDefaultAsync(p => p.Id == pickRequest.Id);

        if (existingPickRequest != null)
        {
            existingPickRequest.RequestNumber = pickRequest.RequestNumber;
            existingPickRequest.OrderId = pickRequest.OrderId;
            existingPickRequest.ClientId = pickRequest.ClientId;
            existingPickRequest.ClientLocationId = pickRequest.ClientLocationId;
            existingPickRequest.RequestedBy = pickRequest.RequestedBy;
            existingPickRequest.RequestDate = pickRequest.RequestDate;
            existingPickRequest.ExecutionDate = pickRequest.ExecutionDate;
            existingPickRequest.Status = pickRequest.Status;
            existingPickRequest.DestinationAddress = pickRequest.DestinationAddress;
            existingPickRequest.DestinationCity = pickRequest.DestinationCity;
            existingPickRequest.Description = pickRequest.Description;
            existingPickRequest.DriverId = pickRequest.DriverId;
            existingPickRequest.VehicleId = pickRequest.VehicleId;
            existingPickRequest.Verified = pickRequest.Verified;
            existingPickRequest.UpdatedAt = DateTime.UtcNow;

            // Synchronize PickRequestLines
            var incomingLineIds = pickRequest.PickRequestLines
                .Where(l => l.Id > 0)
                .Select(l => l.Id)
                .ToHashSet();

            // Remove lines not in incoming list
            var linesToRemove = existingPickRequest.PickRequestLines
                .Where(l => !incomingLineIds.Contains(l.Id))
                .ToList();

            foreach (var lineToRemove in linesToRemove)
            {
                _context.PickRequestLines.Remove(lineToRemove);
            }

            // Update existing lines or add new lines
            foreach (var incomingLine in pickRequest.PickRequestLines)
            {
                if (incomingLine.Id > 0)
                {
                    var existingLine = existingPickRequest.PickRequestLines
                        .FirstOrDefault(l => l.Id == incomingLine.Id);

                    if (existingLine != null)
                    {
                        existingLine.ProductVariantId = incomingLine.ProductVariantId;
                        existingLine.Quantity = incomingLine.Quantity;
                        existingLine.UpdatedAt = DateTime.UtcNow;
                    }
                }
                else
                {
                    existingPickRequest.PickRequestLines.Add(new PickRequestLine
                    {
                        PickRequestId = existingPickRequest.Id,
                        ProductVariantId = incomingLine.ProductVariantId,
                        Quantity = incomingLine.Quantity,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }

            _context.PickRequests.Update(existingPickRequest);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(long id)
    {
        var pickRequest = await _context.PickRequests.FindAsync(id);
        if (pickRequest != null)
        {
            _context.PickRequests.Remove(pickRequest);
            await _context.SaveChangesAsync();
        }
    }
}
