namespace CleanSample.Application.DTOs;

/// <summary>
/// JWT configuration settings
/// </summary>
public class JwtSettingsDto
{
    /// <summary>
    /// JWT secret key
    /// </summary>
    public string SecretKey { get; set; } = null!;

    /// <summary>
    /// JWT issuer
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// JWT audience
    /// </summary>
    public string Audience { get; set; } = null!;

    /// <summary>
    /// Token expiration in minutes
    /// </summary>
    public int ExpirationMinutes { get; set; } = 60;
}