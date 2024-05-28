using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();
        return Ok(departments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> GetDepartmentById(int id)
    {
        var department = await _departmentService.GetDepartmentByIdAsync(id);
        if (department == null) return NotFound();

        return Ok(department);
    }

    [HttpPost]
    public async Task<IActionResult> AddDepartment(Department departmentDto)
    {
        await _departmentService.AddDepartmentAsync(departmentDto);
        return CreatedAtAction(nameof(GetDepartmentById), new { id = departmentDto.DepartmentId }, departmentDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDepartment(int id, Department departmentDto)
    {
        if (id != departmentDto.DepartmentId) return BadRequest();

        await _departmentService.UpdateDepartmentAsync(departmentDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        await _departmentService.DeleteDepartmentAsync(id);
        return NoContent();
    }
}