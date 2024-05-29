using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<DepartmentHistory> _departmentHistoryRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeService(IRepository<Employee> employeeRepository,
            IRepository<DepartmentHistory> departmentHistoryRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentHistoryRepository = departmentHistoryRepository;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            // Mapping to DTO logic
            return employees.Select(e => new Employee
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                HireDate = e.HireDate,
                DepartmentId = e.DepartmentId,
                Phone = e.Phone,
                Address = e.Address,
                IsActive = e.IsActive,
                Department = new Department { DepartmentId = e.Department.DepartmentId, Name = e.Department.Name },
                DepartmentHistories = e.DepartmentHistories?.Select(dh => new DepartmentHistory
                {
                    DepartmentHistoryId = dh.DepartmentHistoryId,
                    EmployeeId = dh.EmployeeId,
                    DepartmentId = dh.DepartmentId,
                    StartDate = dh.StartDate,
                    Department = new Department { DepartmentId = dh.Department.DepartmentId, Name = dh.Department.Name }
                }).ToList() ?? new List<DepartmentHistory>() // Ensure empty list if null
            });
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return null;

            return new Employee
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                HireDate = employee.HireDate,
                DepartmentId = employee.DepartmentId,
                Phone = employee.Phone,
                Address = employee.Address,
                IsActive = employee.IsActive,
                Department = new Department
                {
                    DepartmentId = employee.Department.DepartmentId,
                    Name = employee.Department.Name
                },
                DepartmentHistories = employee.DepartmentHistories?.Select(dh => new DepartmentHistory
                {
                    DepartmentHistoryId = dh.DepartmentHistoryId,
                    EmployeeId = dh.EmployeeId,
                    DepartmentId = dh.DepartmentId,
                    StartDate = dh.StartDate,
                    Department = new Department
                    {
                        DepartmentId = dh.Department.DepartmentId,
                        Name = dh.Department.Name
                    }
                }).ToList() ?? new List<DepartmentHistory>()
            };
        }


        public async Task AddEmployeeAsync(Employee employee)
        {
            await _employeeRepository.AddAsync(employee);

            // Add initial department history
            var departmentHistory = new DepartmentHistory
            {
                EmployeeId = employee.EmployeeId,
                DepartmentId = employee.DepartmentId,
                StartDate = employee.HireDate
            };
            await _departmentHistoryRepository.AddAsync(departmentHistory);
        }

        public async Task UpdateEmployeeAsync(Employee employeeDto)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeDto.EmployeeId);
            if (employee == null)
                return;

            // Check if the department has changed
            if (employee.DepartmentId != employeeDto.DepartmentId)
            {
                var departmentHistory = new DepartmentHistory
                {
                    EmployeeId = employeeDto.EmployeeId,
                    DepartmentId = employeeDto.DepartmentId,
                    StartDate = DateTime.UtcNow // Ensure the date is in UTC
                };
                await _departmentHistoryRepository.AddAsync(departmentHistory);
            }

            // Update employee
            employee.FirstName = employeeDto.FirstName;
            employee.LastName = employeeDto.LastName;
            employee.HireDate = employeeDto.HireDate;
            employee.DepartmentId = employeeDto.DepartmentId;
            employee.Phone = employeeDto.Phone;
            employee.Address = employeeDto.Address;
            employee.IsActive = employeeDto.IsActive;

            await _employeeRepository.UpdateAsync(employee);
        }



        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return;

            await _employeeRepository.DeleteAsync(employee);
        }
    }
}
