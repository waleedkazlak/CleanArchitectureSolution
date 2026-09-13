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
    /// User's full name in English
    /// </summary>
    public string FullNameEn { get; set; } = null!;

    /// <summary>
    /// User's full name in Arabic
    /// </summary>
    public string? FullNameAr { get; set; }

    /// <summary>
    /// User's full name (fallback / compatibility)
    /// </summary>
    public string FullName
    {
        get => FullNameEn;
        set => FullNameEn = value;
    }

    /// <summary>
    /// Email address
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Mobile phone number
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// Hashed password
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Password salt key
    /// </summary>
    public string PasswordSalt { get; set; } = string.Empty;

    /// <summary>
    /// Preferred language code ('en', 'ar')
    /// </summary>
    public string PreferredLanguage { get; set; } = "en";

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; } = true;
}