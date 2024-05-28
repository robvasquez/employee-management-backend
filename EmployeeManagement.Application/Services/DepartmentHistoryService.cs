using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services
{
    public class DepartmentHistoryService : IDepartmentHistoryService
    {
        private readonly IRepository<DepartmentHistory> _departmentHistoryRepository;

        public DepartmentHistoryService(IRepository<DepartmentHistory> departmentHistoryRepository)
        {
            _departmentHistoryRepository = departmentHistoryRepository;
        }

        public async Task<IEnumerable<DepartmentHistory>> GetDepartmentHistoriesByEmployeeIdAsync(int employeeId)
        {
            return await _departmentHistoryRepository.GetAllAsync(dh => dh.EmployeeId == employeeId);
        }

        public async Task AddDepartmentHistoryAsync(DepartmentHistory departmentHistory)
        {
            await _departmentHistoryRepository.AddAsync(departmentHistory);
        }
    }
}