using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

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
    public async Task<ActionResult<Employee>> AddEmployee([FromBody] Employee employee)
    {
        await _employeeService.AddEmployeeAsync(employee);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employeeDto)
    {
        if (id != employeeDto.EmployeeId) return BadRequest("Employee ID mismatch");

        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null) return NotFound();

        if (employee.DepartmentId != employeeDto.Department.DepartmentId)
        {
            employeeDto.DepartmentHistories = employeeDto.DepartmentHistories ?? new List<DepartmentHistory>();
            employeeDto.DepartmentHistories.Add(new DepartmentHistory
            {
                EmployeeId = employeeDto.EmployeeId,
                DepartmentId = employeeDto.Department.DepartmentId,
                StartDate = DateTime.Now
            });
            employeeDto.DepartmentId = employeeDto.Department.DepartmentId;
        }

        await _employeeService.UpdateEmployeeAsync(employeeDto);
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null) return NotFound();

        await _employeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }
}