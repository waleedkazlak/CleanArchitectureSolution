namespace CleanSample.Domain.Entities;

/// <summary>
/// User entity for authentication
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Hashed password
    /// </summary>
    public string PasswordHash { get; set; } = null!;

    /// <summary>
    /// User's full name
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// User role
    /// </summary>
    public string Role { get; set; } = "User";

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Last login timestamp
    /// </summary>
    public DateTime? LastLogin { get; set; }
}