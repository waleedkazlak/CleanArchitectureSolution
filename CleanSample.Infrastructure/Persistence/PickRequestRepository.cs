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
            .Include(p => p.PickRequestLines)
                .ThenInclude(l => l.PickRequestParts)
                    .ThenInclude(prp => prp.Part)
            .Include(p => p.PickRequestParts)
                .ThenInclude(prp => prp.Part)
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
            .Include(p => p.PickRequestLines)
                .ThenInclude(l => l.PickRequestParts)
                    .ThenInclude(prp => prp.Part)
            .Include(p => p.PickRequestParts)
                .ThenInclude(prp => prp.Part)
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
            .Include(p => p.PickRequestLines)
                .ThenInclude(l => l.PickRequestParts)
                    .ThenInclude(prp => prp.Part)
            .Include(p => p.PickRequestParts)
                .ThenInclude(prp => prp.Part)
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
            .Include(p => p.PickRequestLines)
                .ThenInclude(l => l.PickRequestParts)
                    .ThenInclude(prp => prp.Part)
            .Include(p => p.PickRequestParts)
                .ThenInclude(prp => prp.Part)
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
            .Include(p => p.PickRequestLines)
                .ThenInclude(l => l.PickRequestParts)
                    .ThenInclude(prp => prp.Part)
            .Include(p => p.PickRequestParts)
                .ThenInclude(prp => prp.Part)
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
                .ThenInclude(l => l.PickRequestParts)
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

            // Synchronize PickRequestLines and their PickRequestParts
            var incomingLineIds = pickRequest.PickRequestLines
                .Where(l => l.Id > 0)
                .Select(l => l.Id)
                .ToHashSet();

            // Remove lines not in incoming list (cascade removes their PickRequestParts)
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
                        var variantChanged = existingLine.ProductVariantId != incomingLine.ProductVariantId;
                        var quantityChanged = existingLine.Quantity != incomingLine.Quantity;

                        existingLine.ProductVariantId = incomingLine.ProductVariantId;
                        existingLine.Quantity = incomingLine.Quantity;
                        existingLine.UpdatedAt = DateTime.UtcNow;

                        if (variantChanged)
                        {
                            // Remove old parts and regenerate
                            var oldParts = existingLine.PickRequestParts.ToList();
                            foreach (var oldPart in oldParts)
                            {
                                _context.PickRequestParts.Remove(oldPart);
                            }

                            var boms = await _context.ProductBOMs
                                .Where(b => b.ProductVariantId == incomingLine.ProductVariantId)
                                .ToListAsync();

                            foreach (var bom in boms)
                            {
                                existingLine.PickRequestParts.Add(new PickRequestPart
                                {
                                    PickRequestId = existingPickRequest.Id,
                                    PickRequestLineId = existingLine.Id,
                                    PartId = bom.PartId,
                                    RequiredQuantity = incomingLine.Quantity * bom.Quantity,
                                    PickedQuantity = 0,
                                    Status = "Pending",
                                    CreatedAt = DateTime.UtcNow
                                });
                            }
                        }
                        else if (quantityChanged)
                        {
                            // Recalculate required quantity based on BOM
                            var boms = await _context.ProductBOMs
                                .Where(b => b.ProductVariantId == incomingLine.ProductVariantId)
                                .ToListAsync();

                            var bomDict = boms.ToDictionary(b => b.PartId, b => b.Quantity);

                            foreach (var part in existingLine.PickRequestParts)
                            {
                                if (bomDict.TryGetValue(part.PartId, out var bomQty))
                                {
                                    part.RequiredQuantity = incomingLine.Quantity * bomQty;
                                    part.UpdatedAt = DateTime.UtcNow;
                                }
                            }
                        }
                    }
                }
                else
                {
                    var newLine = new PickRequestLine
                    {
                        PickRequestId = existingPickRequest.Id,
                        ProductVariantId = incomingLine.ProductVariantId,
                        Quantity = incomingLine.Quantity,
                        CreatedAt = DateTime.UtcNow
                    };

                    var boms = await _context.ProductBOMs
                        .Where(b => b.ProductVariantId == incomingLine.ProductVariantId)
                        .ToListAsync();

                    foreach (var bom in boms)
                    {
                        newLine.PickRequestParts.Add(new PickRequestPart
                        {
                            PickRequestId = existingPickRequest.Id,
                            PartId = bom.PartId,
                            RequiredQuantity = incomingLine.Quantity * bom.Quantity,
                            PickedQuantity = 0,
                            Status = "Pending",
                            CreatedAt = DateTime.UtcNow
                        });
                    }

                    existingPickRequest.PickRequestLines.Add(newLine);
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
