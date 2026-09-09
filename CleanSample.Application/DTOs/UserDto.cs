namespace CleanSample.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public int? RoleId { get; set; }
    public string? RoleName { get; set; }
    public string UserName { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
    public string? Mobile { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
