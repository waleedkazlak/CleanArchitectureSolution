namespace CleanSample.Application.DTOs;

/// <summary>
/// Login response DTO with JWT token
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// JWT access token
    /// </summary>
    public string AccessToken { get; set; } = null!;

    /// <summary>
    /// Token type (Bearer)
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Token expiration in minutes
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// User information
    /// </summary>
    public UserInfoDto User { get; set; } = new();
}

/// <summary>
/// User information DTO
/// </summary>
public class UserInfoDto
{
    /// <summary>
    /// User ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Full name
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// User role
    /// </summary>
    public string Role { get; set; } = null!;
}