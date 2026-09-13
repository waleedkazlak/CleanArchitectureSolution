using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using CleanSample.Application.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace CleanSample.Application.Services;

/// <summary>
/// Service for handling JWT token generation and validation
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Generates a JWT token for a user with role, permissions, and preferred language
    /// </summary>
    Task<string> GenerateTokenAsync(int userId, string username, string email, string fullName, string role, int? roleId = null, IEnumerable<string>? permissions = null, string? preferredLanguage = null);

    /// <summary>
    /// Validates a JWT token
    /// </summary>
    Task<bool> ValidateTokenAsync(string token);


    /// <summary>
    /// Gets claims from a JWT token
    /// </summary>
    Task<Dictionary<string, string>> GetTokenClaimsAsync(string token);

    /// <summary>
    /// Hashes a password with a randomly generated salt key
    /// </summary>
    (string Hash, string Salt) HashPasswordWithSalt(string password);

    /// <summary>
    /// Verifies a password against a hash and salt
    /// </summary>
    bool VerifyPassword(string password, string hash, string salt);

    /// <summary>
    /// Hashes a password (legacy overload)
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// Verifies a password against a hash (legacy overload)
    /// </summary>
    bool VerifyPassword(string password, string hash);
}


/// <summary>
/// Implementation of authentication service
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly JwtSettingsDto _jwtSettings;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(JwtSettingsDto jwtSettings, ILogger<AuthenticationService> logger)
    {
        _jwtSettings = jwtSettings;
        _logger = logger;
    }

    public async Task<string> GenerateTokenAsync(
        int userId,
        string username,
        string email,
        string fullName,
        string role,
        int? roleId = null,
        IEnumerable<string>? permissions = null,
        string? preferredLanguage = null)
    {
        try
        {
            _logger.LogInformation("Generating JWT token for user: {Username}, role: {Role}", username, role);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var lang = !string.IsNullOrWhiteSpace(preferredLanguage) ? preferredLanguage : "en";

            var claims = new List<System.Security.Claims.Claim>
            {
                new(System.Security.Claims.ClaimTypes.NameIdentifier, userId.ToString()),
                new(System.Security.Claims.ClaimTypes.Name, username),
                new("unique_name", username),
                new(System.Security.Claims.ClaimTypes.Email, email),
                new("email", email),
                new("FullName", fullName),
                new(System.Security.Claims.ClaimTypes.Role, role),
                new("role", role),
                new("preferred_language", lang),
                new("lang", lang),
                new(System.Security.Claims.ClaimTypes.Locality, lang),
                new("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), System.Security.Claims.ClaimValueTypes.Integer64)
            };

            if (roleId.HasValue)
            {
                claims.Add(new System.Security.Claims.Claim("RoleId", roleId.Value.ToString()));
                claims.Add(new System.Security.Claims.Claim("roleId", roleId.Value.ToString()));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("JWT token generated successfully for user: {Username}", username);

            return await Task.FromResult(tokenString);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while generating JWT token for user: {Username}", username);
            throw;
        }
    }


    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token validation failed");
            return await Task.FromResult(false);
        }
    }

    public async Task<Dictionary<string, string>> GetTokenClaimsAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var claims = new Dictionary<string, string>();
            foreach (var claim in jwtToken.Claims)
            {
                claims[claim.Type] = claim.Value;
            }

            return await Task.FromResult(claims);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while extracting claims from token");
            throw;
        }
    }

    public (string Hash, string Salt) HashPasswordWithSalt(string password)
    {
        try
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty", nameof(password));
            }

            byte[] saltBytes = RandomNumberGenerator.GetBytes(16); // 128-bit salt
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                iterations: 100000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);

            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while hashing password with salt");
            throw;
        }
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        try
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(salt))
            {
                return false;
            }

            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] expectedHashBytes = Convert.FromBase64String(hash);
            byte[] actualHashBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                iterations: 100000,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: 32);

            return CryptographicOperations.FixedTimeEquals(actualHashBytes, expectedHashBytes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while verifying password with salt");
            return false;
        }
    }

    public string HashPassword(string password)
    {
        try
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while hashing password");
            throw;
        }
    }

    public bool VerifyPassword(string password, string hash)
    {
        try
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput.Equals(hash);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while verifying password");
            return false;
        }
    }
}