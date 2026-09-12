using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class FieldJobRepository : IFieldJobRepository
{
    private readonly CleanSampleDbContext _context;

    public FieldJobRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public IQueryable<FieldJob> GetQueryable()
    {
        return _context.FieldJobs.AsNoTracking();
    }

    public async Task<FieldJob?> GetByIdAsync(long id)
    {
        return await _context.FieldJobs
            .Include(f => f.LoadRequest)
            .Include(f => f.Client)
            .Include(f => f.ClientLocation)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .FirstOrDefaultAsync(f => f.Id == id);
    }


    public async Task<IEnumerable<FieldJob>> GetAllAsync()
    {
        return await _context.FieldJobs
            .Include(f => f.LoadRequest)
            .Include(f => f.Client)
            .Include(f => f.ClientLocation)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .ToListAsync();
    }

    public async Task<IEnumerable<FieldJob>> GetByLoadRequestIdAsync(long loadRequestId)
    {
        return await _context.FieldJobs
            .Include(f => f.LoadRequest)
            .Include(f => f.Client)
            .Include(f => f.ClientLocation)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .Where(f => f.LoadRequestId == loadRequestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<FieldJob>> GetByClientIdAsync(int clientId)
    {
        return await _context.FieldJobs
            .Include(f => f.LoadRequest)
            .Include(f => f.Client)
            .Include(f => f.ClientLocation)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .Where(f => f.ClientId == clientId)
            .ToListAsync();
    }

    public async Task<IEnumerable<FieldJob>> GetByTechnicianIdAsync(int technicianId)
    {
        return await _context.FieldJobs
            .Include(f => f.LoadRequest)
            .Include(f => f.Client)
            .Include(f => f.ClientLocation)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .Where(f => f.TechnicianId == technicianId)
            .ToListAsync();
    }

    public async Task<IEnumerable<FieldJob>> GetBySupervisorIdAsync(int supervisorId)
    {
        return await _context.FieldJobs
            .Include(f => f.LoadRequest)
            .Include(f => f.Client)
            .Include(f => f.ClientLocation)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .Where(f => f.SupervisorId == supervisorId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(FieldJob fieldJob)
    {
        _context.FieldJobs.Add(fieldJob);
        await _context.SaveChangesAsync();
        return fieldJob.Id;
    }

    public async Task UpdateAsync(FieldJob fieldJob)
    {
        _context.Entry(fieldJob).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var fieldJob = await _context.FieldJobs.FindAsync(id);
        if (fieldJob != null)
        {
            _context.FieldJobs.Remove(fieldJob);
            await _context.SaveChangesAsync();
        }
    }
}
