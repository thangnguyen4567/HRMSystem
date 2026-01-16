using HRMSystem.Data;
using HRMSystem.DTOs;
using HRMSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMSystem.Controllers;

/// <summary>
/// Controller quản lý nhân viên
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmployeesController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy danh sách tất cả nhân viên
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
    {
        var employees = await _context.Employees
            .Where(e => e.IsActive)
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                FullName = e.FullName,
                Email = e.Email,
                Phone = e.Phone,
                Position = e.Position,
                Salary = e.Salary,
                HireDate = e.HireDate,
                IsActive = e.IsActive,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .ToListAsync();

        return Ok(employees);
    }

    /// <summary>
    /// Lấy thông tin nhân viên theo ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null || !employee.IsActive)
        {
            return NotFound("Không tìm thấy nhân viên");
        }

        var employeeDto = new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            FullName = employee.FullName,
            Email = employee.Email,
            Phone = employee.Phone,
            Position = employee.Position,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };

        return Ok(employeeDto);
    }

    /// <summary>
    /// Tạo nhân viên mới
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(CreateEmployeeDto createEmployeeDto)
    {
        // Kiểm tra email đã tồn tại chưa
        if (!string.IsNullOrEmpty(createEmployeeDto.Email))
        {
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == createEmployeeDto.Email && e.IsActive);

            if (existingEmployee != null)
            {
                return BadRequest("Email đã tồn tại trong hệ thống");
            }
        }

        var employee = new Employee
        {
            FirstName = createEmployeeDto.FirstName,
            LastName = createEmployeeDto.LastName,
            Email = createEmployeeDto.Email,
            Phone = createEmployeeDto.Phone,
            Position = createEmployeeDto.Position,
            Salary = createEmployeeDto.Salary,
            HireDate = createEmployeeDto.HireDate,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        var employeeDto = new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            FullName = employee.FullName,
            Email = employee.Email,
            Phone = employee.Phone,
            Position = employee.Position,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };

        return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employeeDto);
    }

    /// <summary>
    /// Cập nhật thông tin nhân viên
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null || !employee.IsActive)
        {
            return NotFound("Không tìm thấy nhân viên");
        }

        // Kiểm tra email đã tồn tại chưa (trừ employee hiện tại)
        if (!string.IsNullOrEmpty(updateEmployeeDto.Email))
        {
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == updateEmployeeDto.Email && e.Id != id && e.IsActive);

            if (existingEmployee != null)
            {
                return BadRequest("Email đã tồn tại trong hệ thống");
            }
        }

        employee.FirstName = updateEmployeeDto.FirstName;
        employee.LastName = updateEmployeeDto.LastName;
        employee.Email = updateEmployeeDto.Email;
        employee.Phone = updateEmployeeDto.Phone;
        employee.Position = updateEmployeeDto.Position;
        employee.Salary = updateEmployeeDto.Salary;
        employee.HireDate = updateEmployeeDto.HireDate;
        employee.IsActive = updateEmployeeDto.IsActive;
        employee.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id))
            {
                return NotFound("Không tìm thấy nhân viên");
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    /// <summary>
    /// Xóa nhân viên (đánh dấu không hoạt động)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound("Không tìm thấy nhân viên");
        }

        employee.IsActive = false;
        employee.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool EmployeeExists(int id)
    {
        return _context.Employees.Any(e => e.Id == id);
    }
}
