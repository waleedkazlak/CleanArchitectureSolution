using CleanSample.Domain.Entities;

namespace CleanSample.Domain.Interfaces;

public interface IRolePermissionRepository
{
    Task<RolePermission?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<RolePermission?> GetByRoleAndScreenIdAsync(int roleId, int screenId, CancellationToken cancellationToken = default);
    Task<RolePermission?> GetByRoleAndScreenCodeAsync(int roleId, string screenCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RolePermission>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RolePermission>> GetByScreenIdAsync(int screenId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RolePermission>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RolePermission> AddAsync(RolePermission rolePermission, CancellationToken cancellationToken = default);
    Task UpdateAsync(RolePermission rolePermission, CancellationToken cancellationToken = default);
    Task DeleteAsync(RolePermission rolePermission, CancellationToken cancellationToken = default);
    Task SetPermissionsBatchAsync(int roleId, IEnumerable<RolePermission> permissions, CancellationToken cancellationToken = default);
}
