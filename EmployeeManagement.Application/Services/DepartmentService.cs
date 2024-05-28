using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;

        public DepartmentService(IRepository<Department> departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
        {
            return await _departmentRepository.GetAllAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task AddDepartmentAsync(Department department)
        {
            await _departmentRepository.AddAsync(department);
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            await _departmentRepository.UpdateAsync(department);
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            await _departmentRepository.DeleteAsync(department);
        }
    }
}