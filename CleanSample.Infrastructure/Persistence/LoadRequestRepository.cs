using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class LoadRequestRepository : ILoadRequestRepository
{
    private readonly CleanSampleDbContext _context;

    public LoadRequestRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<LoadRequest?> GetByIdAsync(long id)
    {
        return await _context.LoadRequests
            .Include(p => p.Order)
            .Include(p => p.Client)
            .Include(p => p.ClientLocation)
            .Include(p => p.Requester)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Include(p => p.LoadRequestLines)
                .ThenInclude(l => l.Product)
            .Include(p => p.LoadRequestLines)
                .ThenInclude(l => l.LoadRequestParts)
                    .ThenInclude(prp => prp.Part)
            .Include(p => p.LoadRequestParts)
                .ThenInclude(prp => prp.Part)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<LoadRequest>> GetAllAsync()
    {
        return await _context.LoadRequests
            .Include(p => p.Order)
            .Include(p => p.Client)
            .Include(p => p.ClientLocation)
            .Include(p => p.Requester)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Include(p => p.LoadRequestLines)
                .ThenInclude(l => l.Product)
            .Include(p => p.LoadRequestLines)
                .ThenInclude(l => l.LoadRequestParts)
                    .ThenInclude(prp => prp.Part)
            .Include(p => p.LoadRequestParts)
                .ThenInclude(prp => prp.Part)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoadRequest>> GetByOrderIdAsync(long orderId)
    {
        return await _context.LoadRequests
            .Include(p => p.Order)
            .Include(p => p.Client)
            .Include(p => p.ClientLocation)
            .Include(p => p.Requester)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Include(p => p.LoadRequestLines)
                .ThenInclude(l => l.Product)
            .Include(p => p.LoadRequestLines)
                .ThenInclude(l => l.LoadRequestParts)
                    .ThenInclude(prp => prp.Part)
            .Include(p => p.LoadRequestParts)
                .ThenInclude(prp => prp.Part)
            .Where(p => p.OrderId == orderId)
            .ToListAsync();
    }

    public async Task<IEnumerable<LoadRequest>> GetByClientIdAsync(int clientId)
    {
        return await _context.LoadRequests
            .Include(p => p.Order)
            .Include(p => p.Client)
            .Include(p => p.ClientLocation)
            .Include(p => p.Requester)
            .Include(p => p.Driver)
            .Include(p => p.Vehicle)
            .Include(p => p.LoadRequestLines)
                .ThenInclude(l => l.Product)
            .Include(p => p.LoadRequestLines)
                .ThenInclude(l => l.LoadRequestParts)
                    .ThenInclude(prp => prp.Part)
            .Include(p => p.LoadRequestParts)
                .ThenInclude(prp => prp.Part)
            .Where(p => p.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(LoadRequest loadRequest)
    {
        _context.LoadRequests.Add(loadRequest);
        await _context.SaveChangesAsync();
        return loadRequest.Id;
    }

    public async Task UpdateAsync(LoadRequest loadRequest)
    {
        var existingLoadRequest = await _context.LoadRequests
            .Include(p => p.LoadRequestLines)
                .ThenInclude(l => l.LoadRequestParts)
            .FirstOrDefaultAsync(p => p.Id == loadRequest.Id);

        if (existingLoadRequest != null)
        {
            if (existingLoadRequest != loadRequest)
            {
                existingLoadRequest.OrderId = loadRequest.OrderId;
                existingLoadRequest.ClientId = loadRequest.ClientId;
                existingLoadRequest.ClientLocationId = loadRequest.ClientLocationId;
                existingLoadRequest.RequestedBy = loadRequest.RequestedBy;
                existingLoadRequest.RequestDate = loadRequest.RequestDate;
                existingLoadRequest.ExecutionDate = loadRequest.ExecutionDate;
                existingLoadRequest.Status = loadRequest.Status;
                existingLoadRequest.DestinationAddress = loadRequest.DestinationAddress;
                existingLoadRequest.DestinationCity = loadRequest.DestinationCity;
                existingLoadRequest.Description = loadRequest.Description;
                existingLoadRequest.DriverId = loadRequest.DriverId;
                existingLoadRequest.VehicleId = loadRequest.VehicleId;
                existingLoadRequest.Verified = loadRequest.Verified;
                existingLoadRequest.UpdatedAt = DateTime.UtcNow;

                // Synchronize LoadRequestLines and their LoadRequestParts
                var incomingLineIds = loadRequest.LoadRequestLines
                    .Where(l => l.Id > 0)
                    .Select(l => l.Id)
                    .ToHashSet();

                // Remove lines not in incoming list (cascade removes their LoadRequestParts)
                var linesToRemove = existingLoadRequest.LoadRequestLines
                    .Where(l => !incomingLineIds.Contains(l.Id))
                    .ToList();

                foreach (var lineToRemove in linesToRemove)
                {
                    _context.LoadRequestLines.Remove(lineToRemove);
                }

                // Update existing lines or add new lines
                var incomingLines = loadRequest.LoadRequestLines.ToList();
                foreach (var incomingLine in incomingLines)
                {
                    if (incomingLine.Id > 0)
                    {
                        var existingLine = existingLoadRequest.LoadRequestLines
                            .FirstOrDefault(l => l.Id == incomingLine.Id);

                        if (existingLine != null)
                        {
                            var productChanged = existingLine.ProductId != incomingLine.ProductId;
                            var quantityChanged = existingLine.Quantity != incomingLine.Quantity;

                            existingLine.ProductId = incomingLine.ProductId;
                            existingLine.Quantity = incomingLine.Quantity;
                            existingLine.UpdatedAt = DateTime.UtcNow;

                            if (productChanged)
                            {
                                // Remove old parts and regenerate
                                var oldParts = existingLine.LoadRequestParts.ToList();
                                foreach (var oldPart in oldParts)
                                {
                                    _context.LoadRequestParts.Remove(oldPart);
                                }

                                var boms = await _context.ProductBOMs
                                    .Where(b => b.ProductId == incomingLine.ProductId)
                                    .ToListAsync();

                                foreach (var bom in boms)
                                {
                                    var partEntity = new LoadRequestPart
                                    {
                                        LoadRequestId = existingLoadRequest.Id,
                                        LoadRequest = existingLoadRequest,
                                        LoadRequestLineId = existingLine.Id,
                                        LoadRequestLine = existingLine,
                                        PartId = bom.PartId,
                                        RequiredQuantity = incomingLine.Quantity * bom.Quantity,
                                        LoadedQuantity = 0,
                                        Status = "Pending",
                                        CreatedAt = DateTime.UtcNow
                                    };
                                    existingLine.LoadRequestParts.Add(partEntity);
                                    existingLoadRequest.LoadRequestParts.Add(partEntity);
                                }
                            }
                            else if (quantityChanged)
                            {
                                // Recalculate required quantity based on BOM
                                var boms = await _context.ProductBOMs
                                    .Where(b => b.ProductId == incomingLine.ProductId)
                                    .ToListAsync();

                                var bomDict = boms.ToDictionary(b => b.PartId, b => b.Quantity);

                                foreach (var part in existingLine.LoadRequestParts)
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
                        var newLine = new CleanSample.Domain.Entities.LoadRequestLine
                        {
                            LoadRequestId = existingLoadRequest.Id,
                            LoadRequest = existingLoadRequest,
                            ProductId = incomingLine.ProductId,
                            Quantity = incomingLine.Quantity,
                            CreatedAt = DateTime.UtcNow
                        };

                        var boms = await _context.ProductBOMs
                            .Where(b => b.ProductId == incomingLine.ProductId)
                            .ToListAsync();

                        foreach (var bom in boms)
                        {
                            var partEntity = new LoadRequestPart
                            {
                                LoadRequestId = existingLoadRequest.Id,
                                LoadRequest = existingLoadRequest,
                                LoadRequestLine = newLine,
                                PartId = bom.PartId,
                                RequiredQuantity = incomingLine.Quantity * bom.Quantity,
                                LoadedQuantity = 0,
                                Status = "Pending",
                                CreatedAt = DateTime.UtcNow
                            };
                            newLine.LoadRequestParts.Add(partEntity);
                            existingLoadRequest.LoadRequestParts.Add(partEntity);
                        }

                        existingLoadRequest.LoadRequestLines.Add(newLine);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(long id)
    {
        var loadRequest = await _context.LoadRequests.FindAsync(id);
        if (loadRequest != null)
        {
            _context.LoadRequests.Remove(loadRequest);
            await _context.SaveChangesAsync();
        }
    }
}
