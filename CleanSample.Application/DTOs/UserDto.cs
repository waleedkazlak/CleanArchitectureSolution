namespace CleanSample.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public int? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string UserName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string FullNameEn { get; set; } = string.Empty;
    public string? FullNameAr { get; set; }
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public string PreferredLanguage { get; set; } = "en";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class UpdatePreferredLanguageDto
{
    public string? PreferredLanguage { get; set; }
    public string? Language { get; set; }

    public string? ResolvedLanguage => !string.IsNullOrWhiteSpace(PreferredLanguage) ? PreferredLanguage : Language;
}

