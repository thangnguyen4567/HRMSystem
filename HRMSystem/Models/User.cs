using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HRMSystem.Models;

/// <summary>
/// Model đại diện cho người dùng hệ thống
/// </summary>
public class User : IdentityUser<int>
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAt { get; set; }

    // Computed property cho tên đầy đủ
    public string FullName => $"{FirstName} {LastName}";
}
