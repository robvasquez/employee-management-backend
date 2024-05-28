using EmployeeManagement.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Interfaces
{
    public interface IDepartmentHistoryService
    {
        Task<IEnumerable<DepartmentHistory>> GetDepartmentHistoriesByEmployeeIdAsync(int employeeId);
        Task AddDepartmentHistoryAsync(DepartmentHistory departmentHistory);
    }
}