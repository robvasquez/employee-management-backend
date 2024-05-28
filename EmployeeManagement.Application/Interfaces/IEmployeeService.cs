using EmployeeManagement.Application.Services;
using EmployeeManagement.Core.Entities;

namespace EmployeeManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee employeeDto);
        Task UpdateEmployeeAsync(Employee employeeDto);
        Task DeleteEmployeeAsync(int id);
    }
}