namespace CleanSample.Domain.Entities;

/// <summary>
/// User entity
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Role identifier
    /// </summary>
    public int? RoleId { get; set; }

    /// <summary>
    /// Role navigation property
    /// </summary>
    public Role? Role { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    public string UserName { get; set; } = null!;

    /// <summary>
    /// Alias for UserName for compatibility
    /// </summary>
    public string Username
    {
        get => UserName;
        set => UserName = value;
    }

    /// <summary>
    /// User's full name
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Mobile phone number
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; } = true;
}