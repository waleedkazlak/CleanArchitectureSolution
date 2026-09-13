using CleanSample.Domain.Entities;
using CleanSample.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanSample.Infrastructure.Persistence;

public class RolePermissionRepository : IRolePermissionRepository
{
    private readonly CleanSampleDbContext _context;

    public RolePermissionRepository(CleanSampleDbContext context)
    {
        _context = context;
    }

    public async Task<RolePermission?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Screen)
            .FirstOrDefaultAsync(rp => rp.Id == id, cancellationToken);
    }

    public async Task<RolePermission?> GetByRoleAndScreenIdAsync(int roleId, int screenId, CancellationToken cancellationToken = default)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Screen)
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.ScreenId == screenId, cancellationToken);
    }

    public async Task<RolePermission?> GetByRoleAndScreenCodeAsync(int roleId, string screenCode, CancellationToken cancellationToken = default)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Screen)
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.Screen.Code == screenCode, cancellationToken);
    }

    public async Task<IReadOnlyList<RolePermission>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Screen)
            .Where(rp => rp.RoleId == roleId)
            .OrderBy(rp => rp.Screen.Module)
            .ThenBy(rp => rp.Screen.NameEn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RolePermission>> GetByScreenIdAsync(int screenId, CancellationToken cancellationToken = default)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Screen)
            .Where(rp => rp.ScreenId == screenId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<RolePermission>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RolePermissions
            .Include(rp => rp.Role)
            .Include(rp => rp.Screen)
            .ToListAsync(cancellationToken);
    }

    public async Task<RolePermission> AddAsync(RolePermission rolePermission, CancellationToken cancellationToken = default)
    {
        await _context.RolePermissions.AddAsync(rolePermission, cancellationToken);
        return rolePermission;
    }

    public Task UpdateAsync(RolePermission rolePermission, CancellationToken cancellationToken = default)
    {
        _context.RolePermissions.Update(rolePermission);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(RolePermission rolePermission, CancellationToken cancellationToken = default)
    {
        _context.RolePermissions.Remove(rolePermission);
        return Task.CompletedTask;
    }

    public async Task SetPermissionsBatchAsync(int roleId, IEnumerable<RolePermission> permissions, CancellationToken cancellationToken = default)
    {
        var existingPermissions = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync(cancellationToken);

        foreach (var incoming in permissions)
        {
            var existing = existingPermissions.FirstOrDefault(ep => ep.ScreenId == incoming.ScreenId);
            if (existing != null)
            {
                existing.CanView = incoming.CanView;
                existing.CanCreate = incoming.CanCreate;
                existing.CanUpdate = incoming.CanUpdate;
                existing.CanDelete = incoming.CanDelete;
                existing.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                await _context.RolePermissions.AddAsync(new RolePermission
                {
                    RoleId = roleId,
                    ScreenId = incoming.ScreenId,
                    CanView = incoming.CanView,
                    CanCreate = incoming.CanCreate,
                    CanUpdate = incoming.CanUpdate,
                    CanDelete = incoming.CanDelete,
                    CreatedAt = DateTime.UtcNow
                }, cancellationToken);
            }
        }
    }
}
