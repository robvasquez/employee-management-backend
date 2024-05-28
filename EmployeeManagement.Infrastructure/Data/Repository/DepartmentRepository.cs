using EmployeeManagement.Core.Entities;
using EmployeeManagement.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Data.Repository
{
    public class DepartmentRepository : IRepository<Department>
    {
        private readonly EmployeeContext _context;

        public DepartmentRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetAllAsync(Expression<Func<Department, bool>> predicate)
        {
            return await _context.Departments.Where(predicate).ToListAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task AddAsync(Department entity)
        {
            await _context.Departments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Department entity)
        {
            _context.Departments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}