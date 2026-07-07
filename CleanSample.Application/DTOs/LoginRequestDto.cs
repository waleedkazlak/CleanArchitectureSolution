namespace CleanSample.Application.DTOs;

/// <summary>
/// Login request DTO
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = null!;
}