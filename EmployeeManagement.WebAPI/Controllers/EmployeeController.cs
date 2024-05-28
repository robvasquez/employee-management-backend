using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployeeById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null) return NotFound();

        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployee(Employee employeeDto)
    {
        await _employeeService.AddEmployeeAsync(employeeDto);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = employeeDto.EmployeeId }, employeeDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, Employee employeeDto)
    {
        if (id != employeeDto.EmployeeId) return BadRequest();

        await _employeeService.UpdateEmployeeAsync(employeeDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await _employeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }
}