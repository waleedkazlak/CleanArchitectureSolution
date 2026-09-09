using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class IssueRepository : IIssueRepository
{
    private readonly CleanSampleDbContext _context;

    public IssueRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<Issue?> GetByIdAsync(long id)
    {
        return await _context.Issues
            .Include(i => i.PickRequest)
            .Include(i => i.FieldJob)
            .Include(i => i.FieldAssembly)
            .Include(i => i.ReportedByUser)
            .Include(i => i.ResolvedByUser)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<Issue>> GetAllAsync()
    {
        return await _context.Issues
            .Include(i => i.PickRequest)
            .Include(i => i.FieldJob)
            .Include(i => i.FieldAssembly)
            .Include(i => i.ReportedByUser)
            .Include(i => i.ResolvedByUser)
            .ToListAsync();
    }

    public async Task<IEnumerable<Issue>> GetByPickRequestIdAsync(long pickRequestId)
    {
        return await _context.Issues
            .Include(i => i.PickRequest)
            .Include(i => i.FieldJob)
            .Include(i => i.FieldAssembly)
            .Include(i => i.ReportedByUser)
            .Include(i => i.ResolvedByUser)
            .Where(i => i.PickRequestId == pickRequestId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Issue>> GetByFieldJobIdAsync(long fieldJobId)
    {
        return await _context.Issues
            .Include(i => i.PickRequest)
            .Include(i => i.FieldJob)
            .Include(i => i.FieldAssembly)
            .Include(i => i.ReportedByUser)
            .Include(i => i.ResolvedByUser)
            .Where(i => i.FieldJobId == fieldJobId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Issue>> GetByFieldAssemblyIdAsync(long fieldAssemblyId)
    {
        return await _context.Issues
            .Include(i => i.PickRequest)
            .Include(i => i.FieldJob)
            .Include(i => i.FieldAssembly)
            .Include(i => i.ReportedByUser)
            .Include(i => i.ResolvedByUser)
            .Where(i => i.FieldAssemblyId == fieldAssemblyId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Issue>> GetByReportedByAsync(int reportedBy)
    {
        return await _context.Issues
            .Include(i => i.PickRequest)
            .Include(i => i.FieldJob)
            .Include(i => i.FieldAssembly)
            .Include(i => i.ReportedByUser)
            .Include(i => i.ResolvedByUser)
            .Where(i => i.ReportedBy == reportedBy)
            .ToListAsync();
    }

    public async Task<IEnumerable<Issue>> GetByResolvedByAsync(int resolvedBy)
    {
        return await _context.Issues
            .Include(i => i.PickRequest)
            .Include(i => i.FieldJob)
            .Include(i => i.FieldAssembly)
            .Include(i => i.ReportedByUser)
            .Include(i => i.ResolvedByUser)
            .Where(i => i.ResolvedBy == resolvedBy)
            .ToListAsync();
    }

    public async Task<long> AddAsync(Issue issue)
    {
        _context.Issues.Add(issue);
        await _context.SaveChangesAsync();
        return issue.Id;
    }

    public async Task UpdateAsync(Issue issue)
    {
        _context.Entry(issue).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var issue = await _context.Issues.FindAsync(id);
        if (issue != null)
        {
            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();
        }
    }
}
