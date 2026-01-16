using System.ComponentModel.DataAnnotations;

namespace HRMSystem.DTOs;

/// <summary>
/// DTO cho việc tạo nhân viên mới
/// </summary>
public class CreateEmployeeDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(200)]
    public string? Position { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Salary { get; set; }

    public DateTime? HireDate { get; set; }
}

/// <summary>
/// DTO cho việc cập nhật nhân viên
/// </summary>
public class UpdateEmployeeDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(200)]
    public string? Position { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Salary { get; set; }

    public DateTime? HireDate { get; set; }

    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO cho phản hồi thông tin nhân viên
/// </summary>
public class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Position { get; set; }
    public decimal? Salary { get; set; }
    public DateTime? HireDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
