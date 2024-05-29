using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers
{
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
        public async Task<ActionResult<Employee>> AddEmployee([FromBody] Employee employee)
        {
            await _employeeService.AddEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateDTO employeeDto)
        {
            if (id != employeeDto.EmployeeId) return BadRequest("Employee ID mismatch");

            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            if (employee.DepartmentId != employeeDto.Department.DepartmentId)
            {
                employeeDto.DepartmentHistories = employeeDto.DepartmentHistories ?? new List<DepartmentHistoryDTO>();
                employeeDto.DepartmentHistories.Add(new DepartmentHistoryDTO
                {
                    EmployeeId = employeeDto.EmployeeId,
                    DepartmentId = employeeDto.Department.DepartmentId,
                    StartDate = DateTime.Now
                });
                employeeDto.DepartmentId = employeeDto.Department.DepartmentId;
            }

            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.HireDate = employeeDto.HireDate;
            employee.Phone = employeeDto.Phone;
            employee.Address = employeeDto.Address;
            employee.IsActive = employeeDto.IsActive;
            employee.DepartmentId = employeeDto.Department.DepartmentId;
            employee.Department = new Department
            {
                DepartmentId = employeeDto.Department.DepartmentId,
                Name = employeeDto.Department.Name
            };
            employee.DepartmentHistories = employeeDto.DepartmentHistories.Select(dh => new DepartmentHistory
            {
                DepartmentHistoryId = dh.DepartmentHistoryId,
                EmployeeId = dh.EmployeeId,
                DepartmentId = dh.DepartmentId,
                StartDate = dh.StartDate
            }).ToList();

            await _employeeService.UpdateEmployeeAsync(employee);

            // Return the updated employee
            return Ok(employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound();

            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }

        [HttpPost("assign-department")]
        public async Task<IActionResult> AssignDepartment([FromBody] AssignDepartmentDTO assignDepartmentDto)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(assignDepartmentDto.EmployeeId);
            if (employee == null) return NotFound();

            // Create a new department history record with UTC start date
            var newDepartmentHistory = new DepartmentHistory
            {
                EmployeeId = employee.EmployeeId,
                DepartmentId = assignDepartmentDto.NewDepartmentId,
                StartDate = DateTime.UtcNow // Ensure the date is in UTC
            };

            // Add the new department history to the employee's department histories
            employee.DepartmentHistories.Add(newDepartmentHistory);

            // Update the employee's department ID
            employee.DepartmentId = assignDepartmentDto.NewDepartmentId;

            // Update the employee in the database
            await _employeeService.UpdateEmployeeAsync(employee);

            // Fetch the updated employee with the new department details
            var updatedEmployee = await _employeeService.GetEmployeeByIdAsync(employee.EmployeeId);

            return Ok(updatedEmployee);
        }
    }
}
