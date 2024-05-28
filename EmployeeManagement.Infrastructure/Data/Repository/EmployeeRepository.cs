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
    public class EmployeeRepository : IRepository<Employee>
    {
        private readonly EmployeeContext _context;

        public EmployeeRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.Include(e => e.Department).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(Expression<Func<Employee, bool>> predicate)
        {
            return await _context.Employees.Include(e => e.Department).Where(predicate).ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task AddAsync(Employee entity)
        {
            await _context.Employees.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee entity)
        {
            _context.Employees.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}