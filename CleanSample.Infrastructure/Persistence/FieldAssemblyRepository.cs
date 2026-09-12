using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class FieldAssemblyRepository : IFieldAssemblyRepository
{
    private readonly CleanSampleDbContext _context;

    public FieldAssemblyRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public IQueryable<FieldAssembly> GetQueryable()
    {
        return _context.FieldAssemblies.AsNoTracking();
    }

    public async Task<FieldAssembly?> GetByIdAsync(long id)
    {
        return await _context.FieldAssemblies
            .Include(f => f.FieldJob)
            .Include(f => f.Product)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<FieldAssembly>> GetAllAsync()
    {
        return await _context.FieldAssemblies
            .Include(f => f.FieldJob)
            .Include(f => f.Product)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .ToListAsync();
    }

    public async Task<IEnumerable<FieldAssembly>> GetByFieldJobIdAsync(long fieldJobId)
    {
        return await _context.FieldAssemblies
            .Include(f => f.FieldJob)
            .Include(f => f.Product)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .Where(f => f.FieldJobId == fieldJobId)
            .ToListAsync();
    }

    public async Task<IEnumerable<FieldAssembly>> GetByProductIdAsync(int productId)
    {
        return await _context.FieldAssemblies
            .Include(f => f.FieldJob)
            .Include(f => f.Product)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .Where(f => f.ProductId == productId)
            .ToListAsync();
    }

    public async Task<IEnumerable<FieldAssembly>> GetByTechnicianIdAsync(int technicianId)
    {
        return await _context.FieldAssemblies
            .Include(f => f.FieldJob)
            .Include(f => f.Product)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .Where(f => f.TechnicianId == technicianId)
            .ToListAsync();
    }

    public async Task<IEnumerable<FieldAssembly>> GetBySupervisorIdAsync(int supervisorId)
    {
        return await _context.FieldAssemblies
            .Include(f => f.FieldJob)
            .Include(f => f.Product)
            .Include(f => f.Technician)
            .Include(f => f.Supervisor)
            .Where(f => f.SupervisorId == supervisorId)
            .ToListAsync();
    }

    public async Task<long> AddAsync(FieldAssembly fieldAssembly)
    {
        _context.FieldAssemblies.Add(fieldAssembly);
        await _context.SaveChangesAsync();
        return fieldAssembly.Id;
    }

    public async Task UpdateAsync(FieldAssembly fieldAssembly)
    {
        _context.Entry(fieldAssembly).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var fieldAssembly = await _context.FieldAssemblies.FindAsync(id);
        if (fieldAssembly != null)
        {
            _context.FieldAssemblies.Remove(fieldAssembly);
            await _context.SaveChangesAsync();
        }
    }
}
